using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Aaron.Core.Bundle;
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

        private Dictionary<long, string> _stringsDictionary = new Dictionary<long, string>();

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
            CarPartPackHeader partPackHeader = BinaryHelpers.ReadStruct<CarPartPackHeader>(reader);

            if (partPackHeader.Version != 6)
            {
                throw new ChunkCorruptedException("Invalid version in CarPartPack header!");
            }

            Debug.WriteLine(
                "Part database: {0} attributes, {1} attribute tables, {2} type names, {3} model tables, {4} parts",
                partPackHeader.NumAttributes, partPackHeader.NumAttributeTables, partPackHeader.NumTypeNames,
                partPackHeader.NumModelTables, partPackHeader.NumParts);
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
    }
}