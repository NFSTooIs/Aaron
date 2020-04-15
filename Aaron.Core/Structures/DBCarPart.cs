using System.Runtime.InteropServices;

namespace Aaron.Core.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DBCarPart
    {
        public byte Unused;
        public short CarIndex;
        public byte Unused2;
        public int AttributeTableOffset;
        public uint Hash;
    }
}