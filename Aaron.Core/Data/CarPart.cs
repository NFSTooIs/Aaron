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
    }
}