namespace Aaron.Core.Attributes
{
    public class LodNamePrefixAttribute : IntAttribute
    {
        public override string GetName()
        {
            return "LOD_NAME_PREFIX_SELECTOR";
        }
    }
}