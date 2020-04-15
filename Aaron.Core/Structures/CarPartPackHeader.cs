using System.Runtime.InteropServices;

namespace Aaron.Core.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CarPartPackHeader
    {
        public uint Version; //=6
        public uint StringTablePointer; //=0
        public uint StringTableSize; //=0
        public uint AttributeTableTablePointer; //=0
        public int NumAttributeTables;
        public uint AttributesTablePointer; //=0
        public int NumAttributes; //=14035
        public uint TypeNameTablePointer; //=0
        public int NumTypeNames;
        public uint ModelTablePointer; //=0
        public int NumModelTables; //=0xA8
        public uint PartsTablePointer; //=0
        public int NumParts;
        public int StartIndex; //=0
    }
}