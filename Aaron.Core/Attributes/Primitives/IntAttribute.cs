using System;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes.Primitives
{
    /// <summary>
    /// Generic attribute that stores an unsigned integer value.
    /// </summary>
    public class IntAttribute : CarPartAttribute
    {
        public IntAttribute(string name)
        {
            Name = name;
        }

        public IntAttribute() {}

        public override string Name { get; }
        public uint Value { get; set; }

        public override IConvertible SaveValue()
        {
            return Value;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            Value = attributeData.UnsignedParam;
        }
    }
}