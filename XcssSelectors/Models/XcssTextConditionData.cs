namespace XcssSelectors.Models
{
    internal class XcssTextConditionData
    {
        public AttributeMatchStyle MatchStyle;

        public string Value;

        public XcssTextConditionData() { }

        public XcssTextConditionData(string value, AttributeMatchStyle matchStyle)
        {
            Value = value;
            MatchStyle = matchStyle;
        }

    }
}
