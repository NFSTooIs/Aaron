using Aaron.Core.Attributes.Primitives;

namespace Aaron.Core.Attributes
{
    public class LodBaseNameAttribute : DoubleStringAttribute
    {
        public override string Name
        {
            get => "LOD_BASE_NAME";
        }
    }
}