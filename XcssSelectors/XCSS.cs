using XcssSelectors.Builders;
using XcssSelectors.Exceptions;
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

        public Xcss Concat(Xcss xcss2)
        {
            if (this._xpaths != null && this._xpaths.Count() > 1)
                throw new XcssException($"Cannot concatenate from a union selector as the root part: '{this.Selector}'");
            if (xcss2._xpaths != null && xcss2._xpaths.Count() > 1)
                throw new XcssException($"Cannot concatenate a union selector as the relative part: '{xcss2.Selector}'");

            string resultXpath = XPathBuilder.Concat(this.XPath, xcss2.XPath);

            IEnumerable<string>? resultXpaths = this._xpaths != null
                ? new[] { resultXpath }
                : null;

            string? resultSelector = this.Selector != null && xcss2.Selector != null
                ? $"{this.Selector} {xcss2.Selector}"
                : null;
            IEnumerable<string>? resultSelectors = resultSelector != null
                ? new[] { resultSelector }
                : null;

            return new Xcss(resultSelectors, resultXpaths, resultSelector, resultXpath);
        }

        public static Xcss Concat(string xcss1, string xcss2) =>
            Parse(xcss1).Concat(Parse(xcss2));

        public static Xcss FromXPath(string xpath)
        {
            return new Xcss(null, null, null, xpath);
        }
    }
}
