using System;
using System.Collections.Generic;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes.Primitives
{
    public abstract class DoubleStringAttribute : StringBasedAttribute
    {
        private ushort _offset1, _offset2;

        public string String1 { get; set; }
        public string String2 { get; set; }

        public override IConvertible SaveValue()
        {
            return (_offset2 << 16) | _offset1;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            _offset1 = (ushort) (attributeData.UnsignedParam & 0xffff);
            _offset2 = (ushort) ((attributeData.UnsignedParam >> 16) & 0xffff);
        }

        public override void ReadStrings(Dictionary<long, string> offsetDictionary)
        {
            String1 = _offset1 != 0xffff ? GetStringByOffset(offsetDictionary, _offset1) : "";
            String2 = _offset2 != 0xffff ? GetStringByOffset(offsetDictionary, _offset2) : "";
        }

        public override void SaveStrings(Dictionary<int, long> hashToOffsetDictionary)
        {
            _offset1 = string.IsNullOrEmpty(String1) 
                ? (ushort) 0xffffu 
                : (ushort)GetOffsetOfString(hashToOffsetDictionary, String1);
            _offset2 = string.IsNullOrEmpty(String2)
                ? (ushort) 0xffffu
                : (ushort)GetOffsetOfString(hashToOffsetDictionary, String2);
        }

        public override List<string> GetStrings()
        {
            return new List<string> { String1, String2 };
        }
    }
}