using System;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes.Primitives
{
    /// <summary>
    /// Generic attribute that stores a float value.
    /// </summary>
    public class FloatAttribute : CarPartAttribute
    {
        public FloatAttribute(string name)
        {
            Name = name;
        }

        public FloatAttribute() {}

        public float Value { get; set; }

        public override string Name { get; }

        public override IConvertible SaveValue()
        {
            return Value;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            Value = attributeData.FloatParam;
        }
    }
}