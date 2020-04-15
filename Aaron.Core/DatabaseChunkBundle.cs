using System.Diagnostics;
using System.IO;
using Aaron.Core.Bundle;
using Aaron.Core.Data;
using Aaron.Core.Loaders;
using Aaron.Core.Structures;
using Aaron.Core.Utils;

namespace Aaron.Core
{
    public class DatabaseChunkBundle : ChunkBundleBase
    {
        private CarPartDatabaseLoader _carPartDatabaseLoader;

        public Database Database { get; }

        public DatabaseChunkBundle(Stream stream) : base(stream)
        {
            Database = new Database();
            _carPartDatabaseLoader = new CarPartDatabaseLoader(Database);
        }

        protected override void ProcessChunk(Chunk chunk, BinaryReader reader)
        {
            switch (chunk.Type)
            {
                case DatabaseChunkIds.CarInfoArrayChunk:
                    ProcessCarInfoArrayChunk(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabaseHeaderChunk:
                    _carPartDatabaseLoader.ProcessHeader(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabaseStringsChunk:
                    _carPartDatabaseLoader.ProcessStrings(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabaseAttributeOffsetTablesChunk:
                    _carPartDatabaseLoader.ProcessAttributeOffsetTablesChunk(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabaseAttributesChunk:
                    _carPartDatabaseLoader.ProcessAttributesChunk(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabaseModelNamesChunk:
                    _carPartDatabaseLoader.ProcessTypeNameHashListChunk(chunk, reader);
                    break;
                case DatabaseChunkIds.CarPartDatabasePartsChunk:
                    _carPartDatabaseLoader.ProcessPartsChunk(chunk, reader);
                    break;
                // Chunks that can be ignored (for now)
                case DatabaseChunkIds.CarPartIndexArrayChunk:
                case DatabaseChunkIds.CarInfoAnimHideTablesChunk:
                case DatabaseChunkIds.CarInfoAnimHookupChunk:
                case DatabaseChunkIds.CarInfoSlotTypesChunk:
                case 0x0: // null
                    break;
                default:
                    Debug.WriteLine($"Cannot process chunk: 0x{chunk.Type:X8}");
                    break;
            }
        }

        private void ProcessCarInfoArrayChunk(Chunk chunk, BinaryReader reader)
        {
            int alignedSize = chunk.GetAlignedSize(0x10);

            if (alignedSize % 0xD0 != 0)
            {
                throw new ChunkCorruptedException("CarTypeInfoArray is malformed!");
            }

            reader.AlignToBoundary(0x10);

            for (int i = 0; i < alignedSize / 0xD0; i++)
            {
                CarTypeInfo carTypeInfo = BinaryHelpers.ReadStruct<CarTypeInfo>(reader);
                CarRecord carRecord = new CarRecord
                {
                    BaseModelName = carTypeInfo.BaseModelName,
                    CarTypeName = carTypeInfo.CarTypeName,
                    DefaultBasePaint = carTypeInfo.DefaultBasePaint,
                    DefaultSkinNumber = carTypeInfo.DefaultSkinNumber,
                    GeometryFilename = carTypeInfo.GeometryFilename,
                    ManufacturerName = carTypeInfo.ManufacturerName,
                    MemoryType = (CarMemoryType)carTypeInfo.CarMemTypeHash,
                    Skinnable = carTypeInfo.Skinnable,
                    UsageType = carTypeInfo.UsageType
                };

                Database.CarRecordManager.AddCarRecord(carRecord);
            }
        }
    }
}