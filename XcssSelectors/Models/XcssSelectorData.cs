namespace XcssSelectors.Models
{
    internal class XcssSelectorData
    {
        public string Selector;

        public XcssSelectorData(string selector)
        {
            Selector = selector;
        }

        public List<XcssElementData> Elements = new List<XcssElementData>();
    }
}
