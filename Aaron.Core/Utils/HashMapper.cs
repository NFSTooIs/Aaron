using System.Collections.Generic;
using System.IO;

namespace Aaron.Core.Utils
{
    public static class HashMapper
    {
        private static readonly Dictionary<uint, string> HashToStringDictionary = new Dictionary<uint, string>();

        public static void LoadStringsFromFile(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (!line.StartsWith('#'))
                {
                    HashToStringDictionary[HashingHelpers.BinHash(line)] = line;
                }
            }
        }

        public static string ResolveHash(uint hash)
        {
            return HashToStringDictionary.TryGetValue(hash, out var s)
                ? s
                : $"0x{hash:X8}";
        }
    }
}