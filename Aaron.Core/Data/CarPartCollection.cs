using System;
using System.Collections.Generic;
using Aaron.Core.Exceptions;

namespace Aaron.Core.Data
{
    /// <summary>
    /// A group of car parts
    /// </summary>
    public class CarPartCollection
    {
        /// <summary>
        /// The name of the collection
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of parts in the collection
        /// </summary>
        public List<CarPart> Parts { get; }

        public CarPartCollection()
        {
            Parts = new List<CarPart>();
        }

        /// <summary>
        /// Finds a car part by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CarPart FindPartByName(string name)
        {
            return Parts.Find(p => string.Equals(p.Name, name, StringComparison.InvariantCulture));
        }

        /// <summary>
        /// Adds a new car part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(CarPart part)
        {
            Parts.Add(part);
        }

        /// <summary>
        /// Removes a car part
        /// </summary>
        /// <param name="part"></param>
        public void RemovePart(CarPart part)
        {
            if (!Parts.Remove(part))
            {
                throw new CarPartNotFoundException($"Part {part.Name} was not found in collection {Name}");
            }
        }
    }
}