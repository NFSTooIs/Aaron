using System;
using Aaron.Core.Data;
using Aaron.Core.Structures;

namespace Aaron.Core.Attributes
{
    public class LanguageHashAttribute : IntAttribute
    {
        public override string GetName()
        {
            return "LANGUAGE_HASH";
        }
    }
}