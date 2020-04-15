using System.Collections.Generic;

namespace Aaron.Core.InternalData
{
    public class AttributeOffsetTable
    {
        public long Offset { get; set; }

        public List<ushort> Offsets { get; set; }
    }
}