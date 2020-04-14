using System;
using System.Collections.Generic;
using System.IO;

namespace Aaron.Core.Bundle
{
    /// <summary>
    /// Base class for chunk bundles that can be read and written
    /// </summary>
    public abstract class ChunkBundleBase
    {
        /// <summary>
        /// The stream to read from or write to.
        /// </summary>
        protected Stream Stream { get; private set; }

        /// <summary>
        /// The list of chunks in the bundle.
        /// </summary>
        /// <remarks>Only valid when reading.</remarks>
        public List<Chunk> Chunks { get; set; }

        private BinaryReader _reader;
        private BinaryWriter _writer;

        protected ChunkBundleBase(Stream stream)
        {
            SetStream(stream);

            this.Chunks = new List<Chunk>();
        }

        /// <summary>
        /// Loads chunks from the stream.
        /// </summary>
        public void Load()
        {
            LoadChunks(Stream.Length);
        }

        /// <summary>
        /// Changes the stream used by the bundle.
        /// </summary>
        /// <param name="stream">The new <see cref="System.IO.Stream"/> instance to use.</param>
        protected void SetStream(Stream stream)
        {
            _reader?.Dispose();
            _writer?.Dispose();
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));

            if (stream.CanRead)
            {
                _reader = new BinaryReader(stream);
            }

            if (stream.CanWrite)
            {
                _writer = new BinaryWriter(stream);
            }
        }

        /// <summary>
        /// Processes a single chunk.
        /// </summary>
        /// <param name="chunk">The chunk to process.</param>
        /// <param name="reader">The <see cref="BinaryReader"/> providing data.</param>
        protected abstract void ProcessChunk(Chunk chunk, BinaryReader reader);

        #region Implementation details

        private void LoadChunks(long length)
        {
            if (_reader == null)
            {
                throw new ChunkBundleException("Cannot load chunks from a non-readable stream");
            }

            var runUntil = Stream.Position + length;

            while (Stream.Position < runUntil)
            {
                uint type = _reader.ReadUInt32();
                int size = _reader.ReadInt32();

                if (size < 0 || Stream.Position + size > runUntil)
                {
                    throw new ChunkBundleException($"Encountered malformed chunk at 0x{Stream.Position:X}");
                }

                long endPos = Stream.Position + size;

                Chunk chunk = new Chunk { Offset = Stream.Position - 8, Size = size, Type = type };
                Chunks.Add(chunk);

                if ((chunk.Type & 0x80000000) == 0x80000000)
                {
                    LoadChunks(size);
                }
                else
                {
                    ProcessChunk(chunk, _reader);
                }

                if (Stream.Position > endPos || Stream.Position < chunk.DataOffset)
                {
                    throw new ChunkBundleException($"Chunk ran out of bounds! Processing 0x{type:X8} ({size} bytes @ 0x{chunk.Offset:X})");
                }

                Stream.Position = endPos;
            }
        }

        #endregion
    }
}