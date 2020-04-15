namespace Aaron.Core.Data
{
    /// <summary>
    /// The basic definition of a car.
    /// </summary>
    public class CarRecord
    {
        public string CarTypeName { get; set; }
        public string BaseModelName { get; set; }
        public string GeometryFilename { get; set; }
        public string ManufacturerName { get; set; }
        public CarUsageType UsageType { get; set; }
        public CarMemoryType MemoryType { get; set; }
        public uint DefaultBasePaint { get; set; }
        public bool Skinnable { get; set; }
        public byte DefaultSkinNumber { get; set; }
    }
}