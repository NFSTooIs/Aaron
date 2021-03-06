﻿using Aaron.Core.Attributes.Primitives;

namespace Aaron.Core.Attributes
{
    public class LodNamePrefixAttribute : IntAttribute
    {
        public override string Name
        {
            get => "LOD_NAME_PREFIX_SELECTOR";
        }
    }
}