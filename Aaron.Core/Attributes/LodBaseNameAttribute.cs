namespace Aaron.Core.Attributes
{
    public class LodBaseNameAttribute : DoubleStringAttribute
    {
        public override string GetName()
        {
            return "LOD_BASE_NAME";
        }
    }
}