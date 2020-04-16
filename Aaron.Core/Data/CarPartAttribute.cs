using System;
using Aaron.Core.Structures;
using Aaron.Core.Utils;

namespace Aaron.Core.Data
{
    /// <summary>
    /// Base class for a car part attribute.
    /// </summary>
    public abstract class CarPartAttribute
    {
        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Gets the name hash of the attribute
        /// </summary>
        /// <returns></returns>
        public virtual uint GetHash() => HashingHelpers.BinHash(GetName());

        /// <summary>
        /// Gets the attribute value to be used in the saving process
        /// </summary>
        /// <returns></returns>
        public abstract IConvertible SaveValue();

        /// <summary>
        /// Loads the attribute value from the raw data structure
        /// </summary>
        /// <param name="attributeData"></param>
        public abstract void LoadValue(CarPartAttributeData attributeData);
    }
}