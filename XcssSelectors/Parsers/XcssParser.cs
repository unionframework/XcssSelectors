using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using XcssSelectors.Models;

namespace XcssSelectors.Parsers
{
    internal class XcssParser
    {
        public static List<Models.XcssSelectorData> Parse(string xcssSelector) {
            AntlrInputStream inputStream = new AntlrInputStream(xcssSelector);
            AntlrXcssLexer xcssLexer = new AntlrXcssLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(xcssLexer);
            AntlrXcssParser xcssParser = new AntlrXcssParser(commonTokenStream);
            var listener = new CollectXcssSelectorsListener();
            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(listener, xcssParser.parse());
            return listener.Selectors;
        }
    }
}
