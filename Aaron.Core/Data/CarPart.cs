using System;
using System.Collections.Generic;

namespace Aaron.Core.Data
{
    /// <summary>
    /// Represents a single part in the part database.
    /// </summary>
    public class CarPart
    {
        /// <summary>
        /// The name of the part.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The part's attributes.
        /// </summary>
        public List<CarPartAttribute> Attributes { get; }

        public CarPart()
        {
            Attributes = new List<CarPartAttribute>();
        }

        /// <summary>
        /// Finds the attribute with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CarPartAttribute FindAttribute(string name)
        {
            return Attributes.Find(a => string.Equals(a.Name, name, StringComparison.InvariantCulture));
        }
    }
}