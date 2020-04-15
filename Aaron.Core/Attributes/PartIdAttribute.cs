using System;
using Aaron.Core.Data;
using Aaron.Core.Enums;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes
{
    public class PartIdAttribute : CarPartAttribute
    {
        public CarPartId PartId { get; set; }
        public byte UnknownValue { get; set; }

        public override string GetName()
        {
            return "PARTID_UPGRADE_GROUP";
        }

        public override IConvertible SaveValue()
        {
            int val = 0;
            val |= (ushort)PartId << 8;
            val |= UnknownValue;

            return val;
        }

        public override void LoadValue(CarPartAttributeData attributeData)
        {
            PartId = (CarPartId)(attributeData.IntParam >> 8);
            UnknownValue = (byte)(attributeData.IntParam & 0xff);
        }
    }
}