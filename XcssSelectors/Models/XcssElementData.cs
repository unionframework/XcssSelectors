namespace XcssSelectors.Models
{
    internal class XcssElementData
    {
        public bool isValidCss;
        public bool isValidXpath;
        public string Combinator;
        public string Xcss;
        public string Tag;
        public string Id;
        public List<string> ClassNames = new List<string>();
        public List<XcssAttributeData> Attributes = new List<XcssAttributeData>();
        public List<XcssTextConditionData> TextConditions = new List<XcssTextConditionData>();
        public List<XcssSelectorData> SubelementConditions = new List<XcssSelectorData>();
        public List<string> SubelementXPathConditions = new List<string>();
        public List<string> Conditions = new List<string>();
        public int? Index;
    }
}
