using System.Runtime.InteropServices;

namespace Aaron.Core.Structures
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct CarPartAttributeData
    {
        [FieldOffset(0)]
        public uint NameHash;

        [FieldOffset(4)]
        public float FloatParam;

        [FieldOffset(4)]
        public int IntParam;

        [FieldOffset(4)]
        public uint UnsignedParam;
    }
}