using System.IO;
using Aaron.Core;
using Aaron.Core.Compression;
using Aaron.Core.Data;
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
        public void TestCarRecordLookup()
        {
            CarRecord carRecord = _database.CarRecordManager.FindCarRecordByName("BMWM3GTR");

            Assert.IsNotNull(carRecord);
            Assert.AreEqual("BMWM3GTR", carRecord.CarTypeName);
            Assert.AreEqual("BMWM3GTR", carRecord.BaseModelName);
            Assert.AreEqual(@"CARS\BMWM3GTR\GEOMETRY.BIN", carRecord.GeometryFilename);
            Assert.AreEqual("BMW", carRecord.ManufacturerName);
            Assert.AreEqual(CarUsageType.Racing, carRecord.UsageType);
            Assert.AreEqual(CarMemoryType.Racing, carRecord.MemoryType);
        }

        [TestMethod]
        public void TestCarPartCollectionLookup()
        {
            CarPartCollection carPartCollection = _database.CarPartManager.FindCarPartCollectionByName("VECTORVINYL");
            Assert.IsNotNull(carPartCollection);
            Assert.AreEqual(3395, carPartCollection.Parts.Count);

            CarPart caymansVinylPart = carPartCollection.FindPartByName("2T_CAYMANS");
            Assert.IsNotNull(caymansVinylPart);
        }
    }
}