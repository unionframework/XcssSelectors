namespace XcssSelectors.Models
{
    internal class XcssAttributeData
    {
        public AttributeMatchStyle MatchStyle;

        public string Name;

        public string Value;

        public XcssAttributeData() { }

        public XcssAttributeData(string name, string value, AttributeMatchStyle matchStyle)
        {
            Name = name;
            Value = value;
            MatchStyle = matchStyle;
        }
    }
}
