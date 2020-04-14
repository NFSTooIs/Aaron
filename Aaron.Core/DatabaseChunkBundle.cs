using System.IO;
using Aaron.Core.Bundle;

namespace Aaron.Core
{
    public class DatabaseChunkBundle : ChunkBundleBase
    {
        public Database Database { get; }

        public DatabaseChunkBundle(Stream stream) : base(stream)
        {
            Database = new Database();
        }

        protected override void ProcessChunk(Chunk chunk, BinaryReader reader)
        {
            switch (chunk.Type)
            {
                case 0:
                    break;
                default:
                    throw new ChunkBundleException($"Cannot process chunk: 0x{chunk.Type:X8}");
            }
        }
    }
}