using Aaron.Core.Attributes.Primitives;

namespace Aaron.Core.Attributes
{
    public class NameOffsetAttribute : SingleStringAttribute
    {
        public override string Name
        {
            get => "NAME_OFFSET";
        }
    }
}