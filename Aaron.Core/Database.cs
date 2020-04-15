using System.Collections.Generic;
using Aaron.Core.Data;
using Aaron.Core.Managers;

namespace Aaron.Core
{
    /// <summary>
    /// Central storage for lists of cars, parts, presets, etc
    /// </summary>
    public class Database
    {
        public CarRecordManager CarRecordManager { get; }

        public Database()
        {
            CarRecordManager = new CarRecordManager(this);
        }
    }
}