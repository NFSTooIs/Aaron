using Aaron.Core.Attributes.Primitives;

namespace Aaron.Core.Attributes
{
    public class TextureAttribute : SingleStringAttribute
    {
        public override string Name
        {
            get => "TEXTURE";
        }
    }
}