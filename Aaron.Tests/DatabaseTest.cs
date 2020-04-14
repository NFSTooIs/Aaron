using System.IO;
using Aaron.Core;
using Aaron.Core.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Tests
{
    [TestClass]
    public class DatabaseTest
    {
        private static Database _database;

        [ClassInitialize]
        public static void SetUpTests(TestContext testContext)
        {
            using FileStream compressedFileStream = new FileStream(@"data\\GlobalC.lzc", FileMode.Open);
            Stream decompressedStream = BlockCompression.StreamBlockFile(compressedFileStream);

            DatabaseChunkBundle chunkBundle = new DatabaseChunkBundle(decompressedStream);
            chunkBundle.Load();

            _database = chunkBundle.Database;
        }

        [TestMethod]
        public void TestSomething()
        {
            Assert.AreEqual(true, true);
        }
    }
}