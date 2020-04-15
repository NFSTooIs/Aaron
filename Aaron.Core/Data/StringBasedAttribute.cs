using System.Collections.Generic;

namespace Aaron.Core.Data
{
    public abstract class StringBasedAttribute : CarPartAttribute
    {
        public List<string> Strings { get; } = new List<string>();

        public abstract void ReadStrings(Dictionary<long, string> offsetDictionary);
        public abstract void SaveStrings(Dictionary<int, long> hashToOffsetDictionary);

        protected string GetStringByOffset(Dictionary<long, string> offsetDictionary, long offset) =>
            offsetDictionary[offset];

        protected long GetOffsetOfString(Dictionary<int, long> hashToOffsetDictionary, string str) =>
            hashToOffsetDictionary[str.GetHashCode()];
    }
}