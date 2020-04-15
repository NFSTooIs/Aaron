using System;
using System.Collections.Generic;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes
{
    public class LodBaseNameAttribute : DoubleStringAttribute
    {
        public override string GetName()
        {
            return "LOD_BASE_NAME";
        }
    }
}