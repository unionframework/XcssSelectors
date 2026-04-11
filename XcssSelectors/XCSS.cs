using XcssSelectors.Builders;
using XcssSelectors.Parsers;

namespace XcssSelectors
{
    public class Xcss
    {
        private readonly IEnumerable<string> _selectors;
        private readonly IEnumerable<string> _xpaths;
        public readonly string Selector;
        public readonly string XPath;
        public readonly bool ConcatWithRoot;

        private Xcss(IEnumerable<string> selectors, IEnumerable<string> xpaths, string combinedSelector, string combinedXpath, bool concatWithRoot = false)
        {
            _selectors = selectors;
            _xpaths = xpaths;
            Selector = combinedSelector;
            XPath = combinedXpath;
            ConcatWithRoot = concatWithRoot;
        }

        public static Xcss Parse(string combinedXcss, XcssOptions options = XcssOptions.None)
        {
            var selectorsData = XcssParser.Parse(combinedXcss);
            IEnumerable<string> selectors = selectorsData.Select(s => s.Selector);
            IEnumerable<string> xpaths = XPathBuilder.Build(selectorsData, options);
            string combinedXpath = XPathBuilder.Combine(xpaths);
            return new Xcss(selectors, xpaths, combinedXcss, combinedXpath);
        }

        //public Xcss Concat(Xcss xcss2)
        //{
        //    string resultXpath = XPathBuilder.Concat(XPath, xcss2.XPath);
        //    return new Xcss(null, resultXpath);
        //}
    }
}
