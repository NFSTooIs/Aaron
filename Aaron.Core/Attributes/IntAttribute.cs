using System;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes
{
    /// <summary>
    /// Generic attribute that stores an unsigned integer value.
    /// </summary>
    public class IntAttribute : CarPartAttribute
    {
        public uint Hash { get; set; }
        public uint Value { get; set; }

        public IntAttribute(uint hash)
        {
            Hash = hash;
        }

        public IntAttribute() { }

        public override string GetName()
        {
            return $"0x{Hash:X8}";
        }

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