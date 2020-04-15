using System.Collections.Generic;

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
    }
}