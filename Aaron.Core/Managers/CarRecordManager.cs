using System;
using System.Collections.Generic;
using Aaron.Core.Data;

namespace Aaron.Core.Managers
{
    public class CarRecordManager
    {
        private readonly Database _database;

        /// <summary>
        /// The list of car records
        /// </summary>
        public List<CarRecord> CarRecords { get; }

        public CarRecordManager(Database database)
        {
            _database = database;
            CarRecords = new List<CarRecord>();
        }

        /// <summary>
        /// Adds a new car record to the list of car records.
        /// </summary>
        /// <param name="carRecord"></param>
        public void AddCarRecord(CarRecord carRecord)
        {
            CarRecords.Add(carRecord);
        }

        /// <summary>
        /// Finds the car record with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CarRecord FindCarRecordByName(string name)
        {
            return CarRecords.Find(c => 
                string.Equals(c.CarTypeName, name, StringComparison.InvariantCulture));
        }
    }
}