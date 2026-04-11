using XcssSelectors.Models;

namespace XcssSelectors.Parsers
{
    internal class XcssSelectorContext
    {
        public XcssTextConditionData Text = new XcssTextConditionData();
        public XcssAttributeData Attribute = new XcssAttributeData();
        public XcssElementData Element = new XcssElementData();
        public XcssSelectorData Selector;

        public XcssSelectorContext(string originalSelector)
        {
            Selector = new XcssSelectorData(originalSelector);
        }

        private int _xpathConditionLevel = 0;

        internal void StartXpathCondition(string xpathCondition)
        {
            if (_xpathConditionLevel == 0)
            {
                Element.SubelementXPathConditions.Add(xpathCondition);
            }
            _xpathConditionLevel++;
        }

        internal void FinishXpathCondition()
        {
            _xpathConditionLevel--;
        }

    }
}