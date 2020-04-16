using System;
using System.IO;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes.Primitives
{
    /// <summary>
    /// Generic attribute that stores a boolean value.
    /// </summary>
    public class BoolAttribute : CarPartAttribute
    {
        public BoolAttribute(string name)
        {
            Name = name;
        }

        public BoolAttribute() { }

        public override string Name { get; }

        public bool Value { get; set; }

        public override IConvertible SaveValue()
        {
            return Value ? 1 : 0;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            if (attributeData.UnsignedParam > 1)
                throw new InvalidDataException("Boolean value cannot be > 1");
            Value = attributeData.UnsignedParam == 1;
        }
    }
}