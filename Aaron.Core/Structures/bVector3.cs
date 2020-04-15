using System.Runtime.InteropServices;

namespace Aaron.Core.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct bVector3
    {
        public float X;
        public float Y;
        public float Z;
        public float Pad;
    }
}