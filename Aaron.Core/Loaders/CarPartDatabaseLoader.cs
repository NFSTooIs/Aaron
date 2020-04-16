using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aaron.Core.Attributes;
using Aaron.Core.Attributes.Primitives;
using Aaron.Core.Bundle;
using Aaron.Core.Data;
using Aaron.Core.InternalData;
using Aaron.Core.Structures;
using Aaron.Core.Utils;

namespace Aaron.Core.Loaders
{
    /// <summary>
    /// Loaders for car part database chunks
    /// </summary>
    public class CarPartDatabaseLoader
    {
        private readonly Database _database;

        private List<CarPartAttributeData> _attributes = new List<CarPartAttributeData>();
        private Dictionary<long, AttributeOffsetTable> _attributeOffsetTables =
            new Dictionary<long, AttributeOffsetTable>();
        private List<uint> _typeNameHashes = new List<uint>();
        private List<DBCarPart> _parts = new List<DBCarPart>();
        private Dictionary<long, string> _stringsDictionary = new Dictionary<long, string>();
        private CarPartPackHeader _header;
        private List<CarPartAttribute> _preparedAttributes = new List<CarPartAttribute>();

        public CarPartDatabaseLoader(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// Processes the car part database header chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessHeader(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size != 0x40)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack header!");
            }

            reader.BaseStream.Seek(8, SeekOrigin.Current);
            _header = BinaryHelpers.ReadStruct<CarPartPackHeader>(reader);

            if (_header.Version != 6)
            {
                throw new ChunkCorruptedException("Invalid version in CarPartPack header!");
            }

            Debug.WriteLine(
                "Part database: {0} attributes, {1} attribute tables, {2} type names, {3} model tables, {4} parts",
                _header.NumAttributes, _header.NumAttributeTables, _header.NumTypeNames,
                _header.NumModelTables, _header.NumParts);
        }

        /// <summary>
        /// Processes the car part database strings chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessStrings(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size % 4 != 0)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack strings chunk!");
            }

            while (reader.BaseStream.Position < chunk.EndOffset)
            {
                var offset = reader.BaseStream.Position - chunk.DataOffset;
                _stringsDictionary.Add(offset, reader.ReadAlignedString());
            }

            Debug.WriteLine("Loaded {0} strings", _stringsDictionary.Count);
        }


        /// <summary>
        /// Processes the attribute offset table list chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessAttributeOffsetTablesChunk(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size % 2 != 0)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack attribute offset tables chunk!");
            }

            while (reader.BaseStream.Position < chunk.EndOffset)
            {
                var table = new AttributeOffsetTable
                {
                    Offset = (reader.BaseStream.Position - chunk.DataOffset) / 2
                };
                var numOffsets = reader.ReadUInt16();
                table.Offsets = new List<ushort>(numOffsets);

                for (var i = 0; i < numOffsets; i++)
                {
                    table.Offsets.Add(reader.ReadUInt16());
                }

                _attributeOffsetTables[table.Offset] = table;
            }

            Debug.WriteLine("Loaded {0} attribute offset tables", _attributeOffsetTables.Count);
        }

        /// <summary>
        /// Processes the attribute list chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessAttributesChunk(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size % 8 != 0)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack attributes chunk!");
            }

            var numAttributes = chunk.Size >> 3;
            
            if (numAttributes != _header.NumAttributes)
            {
                throw new ChunkCorruptedException($"CarPartPack header says there are {_header.NumAttributes} attributes, but the attribute list has {numAttributes}!");
            }

            for (int i = 0; i < numAttributes; i++)
            {
                CarPartAttributeData carPartAttribute = BinaryHelpers.ReadStruct<CarPartAttributeData>(reader);
                _attributes.Add(carPartAttribute);
            }

            PrepareAttributes();
            Debug.WriteLine("Loaded {0} attributes", _attributes.Count);
        }

        /// <summary>
        /// Processes the type name hash list chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessTypeNameHashListChunk(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size % 4 != 0)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack type name hash list chunk!");
            }

            var numTypeNameHashes = chunk.Size >> 2;

            if (numTypeNameHashes != _header.NumTypeNames)
            {
                throw new ChunkCorruptedException(
                    $"CarPartPack header says there are {_header.NumTypeNames} type name hashes, but the type name hash list has {numTypeNameHashes}!");
            }

            for (int i = 0; i < numTypeNameHashes; i++)
            {
                _typeNameHashes.Add(reader.ReadUInt32());
            }

            Debug.WriteLine("Loaded {0} type name hashes", _typeNameHashes.Count);
        }

        /// <summary>
        /// Processes the part list chunk
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="reader"></param>
        public void ProcessPartsChunk(Chunk chunk, BinaryReader reader)
        {
            if (chunk.Size % 0xC != 0)
            {
                throw new ChunkCorruptedException("Invalid CarPartPack part list chunk!");
            }

            int numParts = chunk.Size / 0xC;

            if (numParts != _header.NumParts)
            {
                throw new ChunkCorruptedException($"CarPartPack header says there are {_header.NumParts} parts, but the part list has {numParts}!");
            }

            for (int i = 0; i < numParts; i++)
            {
                _parts.Add(BinaryHelpers.ReadStruct<DBCarPart>(reader));
            }

            Debug.WriteLine("Loaded {0} parts", _parts.Count);
            GeneratePartCollections();
            Debug.WriteLine("Generated part collections");
        }

        private void GeneratePartCollections()
        {
            foreach (var partGroup in _parts.GroupBy(p => p.CarIndex))
            {
                var partCollection = new CarPartCollection
                {
                    Name = HashMapper.ResolveHash(_typeNameHashes[partGroup.Key])
                };

                foreach (var dbCarPart in partGroup)
                {
                    var carPart = new CarPart
                    {
                        Name = HashMapper.ResolveHash(dbCarPart.Hash)
                    };

                    LoadPartAttributes(carPart, dbCarPart);

                    partCollection.Parts.Add(carPart);
                }

                _database.CarPartManager.AddCarPartCollection(partCollection);
            }
        }

        private void PrepareAttributes()
        {
            foreach (var attribute in _attributes)
            {
                _preparedAttributes.Add(GetPreparedAttribute(attribute));
            }
        }

        private CarPartAttribute GetPreparedAttribute(CarPartAttributeData attributeData)
        {
            var attributeName = HashMapper.ResolveHash(attributeData.NameHash);
            CarPartAttribute attribute;

            switch (attributeName)
            {
                case "PARTID_UPGRADE_GROUP":
                    attribute = new PartIdAttribute();
                    break;
                case "LANGUAGEHASH":
                    attribute = new LanguageHashAttribute();
                    break;
                case "KITNUMBER":
                    attribute = new KitNumberAttribute();
                    break;
                case "LOD_NAME_PREFIX_SELECTOR":
                    attribute = new LodNamePrefixAttribute();
                    break;
                case "LOD_BASE_NAME":
                    attribute = new LodBaseNameAttribute();
                    break;
                case "NAME_OFFSET":
                    attribute = new NameOffsetAttribute();
                    break;
                case "LOD_CHARACTERS_OFFSET":
                    attribute = new LodCharactersOffsetAttribute();
                    break;
                case "LOD_NAME_PREFIX_NAMEHASH":
                    attribute = new LodNamePrefixNameHashAttribute();
                    break;
                case "ONLINE":
                case "STOCK":
                case "CARBONFIBRE":
                case "CENTER":
                case "0x87557E1E":
                case "0xF9661A07":
                case "STOCK_MATERIAL":
                case "USEMARKER1":
                case "FULLBODY":
                case "MIRROR":
                case "ISDECAL":
                    attribute = new BoolAttribute(attributeName);
                    break;
                case "TEXTURE":
                    attribute = new TextureAttribute();
                    break;
                case "0x10C98090":
                case "MAX_LOD":
                case "COLOR0ID":
                case "COLOR1ID":
                case "COLOR2ID":
                case "COLOR3ID":
                case "VINYLLANGUAGEHASH":
                case "TEXTUREHASH":
                case "CV":
                case "SPECIFICCARNAME":
                case "0x6BA02C05":
                case "MAT0":
                case "MAT1":
                case "MAT2":
                case "MAT3":
                case "MAT4":
                case "MAT5":
                case "MAT6":
                case "MAT7":
                case "0xC9818DFC":
                case "0xEBB03E66":
                case "MATNAMEA":
                case "MATNAMEB":
                case "0xD68A7BAB":
                case "RED":
                case "GREEN":
                case "BLUE":
                case "SWATCH":
                case "GROUPLANGUAGEHASH":
                case "0x04B39858":
                case "0x5412A1D9":
                case "MODEL_TABLE_OFFSET":
                case "MORPHTARGET_NUM":
                case "0xCE7D8DB5":
                case "0x7D29CF3E":
                case "0x65F58556":
                case "GLOSS":
                case "0xB5548ED7":
                case "0x1B0EA1A9":
                case "0xEB0101E2":
                case "0xE80A3B62":
                case "PAINTGROUP":
                case "DAMAGELEVEL":
                    attribute = new IntAttribute(attributeName);
                    break;
                case "BLEND":
                case "0x9A9B6DDC":
                    attribute = new FloatAttribute(attributeName);
                    break;
                default:
                    attribute = new IntAttribute(attributeName);
                    Debug.WriteLine("WARNING: Unimplemented attribute {0}; value={1} (0x{1:X8})", attributeName, attributeData.UnsignedParam);
                    break;
            }

            attribute.LoadValue(attributeData);

            if (attribute is StringBasedAttribute sba)
            {
                sba.ReadStrings(_stringsDictionary);
            }

            return attribute;
        }

        private void LoadPartAttributes(CarPart carPart, DBCarPart carPartInfo)
        {
            AttributeOffsetTable attributeOffsetTable = _attributeOffsetTables[carPartInfo.AttributeTableOffset];

            foreach (var attributeOffset in attributeOffsetTable.Offsets)
            {
                var attribute = _preparedAttributes[attributeOffset];

                carPart.Attributes.Add(attribute);
            }
        }
    }
}