namespace Aaron.Core.Bundle
{
    /// <summary>
    /// A data chunk in a bundle.
    /// </summary>
    /// <remarks>Raw data is not stored.</remarks>
    public class Chunk
    {
        /// <summary>
        /// The chunk type identifier
        /// </summary>
        public uint Type { get; set; }

        /// <summary>
        /// The length of the chunk data
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The offset of the chunk
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// The offset of the chunk data
        /// </summary>
        public long DataOffset => Offset + 8;

        /// <summary>
        /// The offset of the end of the chunk
        /// </summary>
        public long EndOffset => DataOffset + Size;
    }
}