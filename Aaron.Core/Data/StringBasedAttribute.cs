using System.Collections.Generic;

namespace Aaron.Core.Data
{
    public abstract class StringBasedAttribute : CarPartAttribute
    {
        public abstract void ReadStrings(Dictionary<long, string> offsetDictionary);
        public abstract void SaveStrings(Dictionary<int, long> hashToOffsetDictionary);
        public abstract List<string> GetStrings();

        protected string GetStringByOffset(Dictionary<long, string> offsetDictionary, long offset) =>
            offsetDictionary[offset * 4];

        protected long GetOffsetOfString(Dictionary<int, long> hashToOffsetDictionary, string str) =>
            hashToOffsetDictionary[str.GetHashCode()] / 4;
    }
}