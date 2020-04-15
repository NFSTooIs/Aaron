using System;
using System.Collections.Generic;
using Aaron.Core.Data;

namespace Aaron.Core.Managers
{
    public class CarPartManager
    {
        private readonly Database _database;

        /// <summary>
        /// The list of car part collections.
        /// </summary>
        public List<CarPartCollection> CarPartCollections { get; }

        public CarPartManager(Database database)
        {
            _database = database;
            CarPartCollections = new List<CarPartCollection>();
        }

        /// <summary>
        /// Adds a new car part collection to the list of car part collections.
        /// </summary>
        /// <param name="carPartCollection"></param>
        public void AddCarPartCollection(CarPartCollection carPartCollection)
        {
            CarPartCollections.Add(carPartCollection);
        }

        /// <summary>
        /// Finds the car part collection with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CarPartCollection FindCarPartCollectionByName(string name)
        {
            return CarPartCollections.Find(c =>
                string.Equals(c.Name, name, StringComparison.InvariantCulture));
        }
    }
}