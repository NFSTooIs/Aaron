using System.IO;
using Aaron.Core.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Tests
{
    [TestClass]
    public class BlockCompressionTest
    {
        [TestMethod]
        public void TestDecompressionToArray()
        {
            using FileStream compressedFileStream = new FileStream(@"data\\GlobalC.lzc", FileMode.Open);
            byte[] decompressedBytes = BlockCompression.ReadBlockFile(compressedFileStream);

            Assert.AreEqual(9963360, decompressedBytes.Length);
        }

        [TestMethod]
        public void TestDecompressionToStream()
        {
            using FileStream compressedFileStream = new FileStream(@"data\\GlobalC.lzc", FileMode.Open);
            Stream decompressedStream = BlockCompression.StreamBlockFile(compressedFileStream);

            Assert.AreEqual(9963360, decompressedStream.Length);
            Assert.AreEqual(0, decompressedStream.Position);
        }

        [TestMethod]
        public void TestCompression()
        {
            MemoryStream ms = new MemoryStream();
            byte[] data =
            {
                0x1, 0x2, 0x3, 0x4, 0x4, 0x4, 0x4, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x5, 0x6, 0x5, 0x5, 0x6, 0x5, 0x5, 0x7,
                0x1, 0x2, 0x3, 0x4, 0x4, 0x4, 0x4, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x5, 0x6, 0x5, 0x5, 0x6, 0x5, 0x5, 0x7,
                0x1, 0x2, 0x3, 0x4, 0x4, 0x4, 0x4, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x5, 0x6, 0x5, 0x5, 0x6, 0x5, 0x5, 0x7,
                0x1, 0x2, 0x3, 0x4, 0x4, 0x4, 0x4, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x5, 0x6, 0x5, 0x5, 0x6, 0x5, 0x5, 0x7,
            };
            BlockCompression.WriteCompressedBlocks(ms, data);
        }
    }
}