using System.IO;
using Aaron.Core.Bundle;
using Aaron.Core.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Tests
{
    [TestClass]
    public class ChunkBundleTests
    {
        [TestMethod]
        public void TestBasicBundleLoad()
        {
            using FileStream compressedFileStream = new FileStream(@"data\\GlobalC.lzc", FileMode.Open);
            Stream decompressedStream = BlockCompression.StreamBlockFile(compressedFileStream);

            BasicChunkBundle basicChunkBundle = new BasicChunkBundle(decompressedStream);
            basicChunkBundle.Load();

            Assert.AreEqual(basicChunkBundle.Chunks.Count, 227);
        }
    }
}
