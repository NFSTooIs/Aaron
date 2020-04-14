using System.IO;

namespace Aaron.Core.Bundle
{
    public class BasicChunkBundle : ChunkBundleBase
    {
        public BasicChunkBundle(Stream stream) : base(stream)
        {
        }

        protected override void ProcessChunk(Chunk chunk, BinaryReader reader)
        {
            //
        }
    }
}