using System;
using System.Collections.Generic;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes.Primitives
{
    public abstract class SingleStringAttribute : StringBasedAttribute
    {
        private uint _offset;

        public string Value { get; set; }

        public override IConvertible SaveValue()
        {
            return  _offset;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            _offset = attributeData.UnsignedParam;
        }

        public override void ReadStrings(Dictionary<long, string> offsetDictionary)
        {
            Value = _offset != 0xffffffff ? GetStringByOffset(offsetDictionary, _offset) : "";
        }

        public override void SaveStrings(Dictionary<int, long> hashToOffsetDictionary)
        {
            _offset = string.IsNullOrEmpty(Value) 
                ? 0xffffffffu
                : (uint) GetOffsetOfString(hashToOffsetDictionary, Value);
        }

        public override List<string> GetStrings()
        {
            return new List<string> { Value };
        }
    }
}