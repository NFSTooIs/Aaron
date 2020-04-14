using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aaron.Core.Utils;

namespace Aaron.Core.Compression
{
    /// <summary>
    /// Operates on block-compressed (.lzc) files.
    /// </summary>
    public static class BlockCompression
    {
        private class CompressedBlock
        {
            public int OutSize { get; set; }
            public int UPos { get; set; }
            public int CPos { get; set; }
            public byte[] DataRaw { get; set; }
            public byte[] DataComp { get; set; }
        }

        /// <summary>
        /// Reads compressed blocks from the given stream and returns a <see cref="MemoryStream"/> containing the decompressed contents.
        /// </summary>
        /// <param name="stream">The stream to read compressed blocks from.</param>
        /// <returns>A new <see cref="MemoryStream"/> containing the decompressed data.</returns>
        public static MemoryStream StreamBlockFile(Stream stream)
        {
            return new MemoryStream(ReadBlockFile(stream));
        }

        /// <summary>
        /// Reads a compressed-in-place file. This involves reading a header and then
        /// proceeding to a shared routine for decompression.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadBlockFile(Stream stream)
        {
            var br = new BinaryReader(stream);
            var fileHeader = BinaryHelpers.ReadStruct<CIPFileHeader>(br);

            if (fileHeader.Magic != 0x66113388)
            {
                throw new InvalidDataException("Invalid header");
            }

            return ReadCompressedBlocks(stream, fileHeader.USize);
        }

        /// <summary>
        /// Reads compressed-in-place blocks from a stream. No header is read.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ReadCompressedBlocks(Stream stream, int size)
        {
            var data = new byte[size];

            BlockDecompress(stream, data);

            return data;
        }

        /// <summary>
        /// Writes a compressed-in-place file. Compresses the input data as blocks,
        /// then writes a header followed by the blocks.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="blockSize"></param>
        public static void WriteBlockFile(Stream stream, IEnumerable<byte> data, int blockSize = 32768)
        {
            if ((blockSize & (blockSize - 1)) != 0)
            {
                throw new ArgumentException("blockSize must be a power of two!");
            }

            var blocksSorted = BlockCompress(data, blockSize);

            using var bw = new BinaryWriter(stream);
            var header = new CIPFileHeader
            {
                Magic = 0x66113388,
                CSize = blocksSorted.Sum(b => b.DataComp.Length + 24),
                USize = blocksSorted.Sum(b => b.DataRaw.Length),
                Unknown = 0
            };

            BinaryHelpers.WriteStruct(bw, header);

            WriteBlocksInternal(blocksSorted, bw);
        }

        /// <summary>
        /// Writes compressed-in-place blocks to a stream, consisting of compressed input data.
        /// No header is written.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="blockSize"></param>
        public static void WriteCompressedBlocks(Stream stream, IEnumerable<byte> data, int blockSize = 32768)
        {
            if ((blockSize & (blockSize - 1)) != 0)
            {
                throw new ArgumentException("blockSize must be a power of two!");
            }

            var blocksSorted = BlockCompress(data, blockSize);

            using var bw = new BinaryWriter(stream);
            WriteBlocksInternal(blocksSorted, bw);
        }

        private static void WriteBlocksInternal(IEnumerable<CompressedBlock> blocksSorted, BinaryWriter bw)
        {
            foreach (var compressedBlock in blocksSorted)
            {
                var cbh = new CIPHeader
                {
                    Magic = 0x55441122,
                    CPos = compressedBlock.CPos,
                    USize = compressedBlock.DataRaw.Length,
                    CSize = 24 + compressedBlock.DataComp.Length,
                    UPos = compressedBlock.UPos
                };

                BinaryHelpers.WriteStruct(bw, cbh);
                bw.Write(compressedBlock.DataComp);
            }
        }

        private static void BlockDecompress(Stream stream, byte[] outData)
        {
            while (stream.Position < stream.Length)
            {
                var header = BinaryHelpers.ReadStruct<CIPHeader>(stream);

                if (header.Magic != 0x55441122)
                {
                    throw new InvalidDataException($"Invalid magic! Expected 0x55441122, got 0x{header.Magic:X8}");
                }

                var data = new byte[header.CSize - 24];
                var decompressed = new byte[header.USize];

                if (stream.Read(data, 0, data.Length) != data.Length)
                {
                    throw new InvalidDataException($"Failed to read {data.Length} bytes from stream");
                }

                Compressor.Decompress(data, decompressed);

                Array.ConstrainedCopy(
                    decompressed,
                    0,
                    outData,
                    header.UPos,
                    decompressed.Length);
            }
        }

        private static List<CompressedBlock> BlockCompress(IEnumerable<byte> data, int blockSize = 32768)
        {
            var parts = data.Clump(blockSize).Select(p => p.ToList()).ToList();
            var outBlocks = new List<CompressedBlock>();
            var uncompressedPos = 0;

            foreach (var part in parts)
            {
                var outBlock = new CompressedBlock
                {
                    DataRaw = part.ToArray(),
                    UPos = uncompressedPos,
                    OutSize = part.Count
                };

                var compData = new byte[part.Count * 2];
                Compressor.Compress(outBlock.DataRaw, ref compData);

                outBlock.DataComp = compData;

                outBlocks.Add(outBlock);
                uncompressedPos += outBlock.DataRaw.Length;
            }

            var blocksSorted = new List<CompressedBlock>(outBlocks.Count);

            if (blocksSorted.Count == 1)
            {
                blocksSorted.Add(outBlocks[0]);
            }
            else
            {
                blocksSorted.Add(outBlocks[1]);

                for (var i = 2; i < outBlocks.Count; i++)
                {
                    blocksSorted.Add(outBlocks[i]);
                }

                blocksSorted.Add(outBlocks[0]);

                var compPos = 0;

                foreach (var outBlock in blocksSorted)
                {
                    outBlock.CPos = compPos;

                    if (outBlock.DataComp.Length % 4 != 0)
                    {
                        var tmpdc = outBlock.DataComp;
                        Array.Resize(ref tmpdc, tmpdc.Length + (4 - tmpdc.Length % 4));
                        outBlock.DataComp = tmpdc;
                    }

                    compPos += outBlock.DataComp.Length + 24;
                }
            }

            return blocksSorted;
        }
    }
}
