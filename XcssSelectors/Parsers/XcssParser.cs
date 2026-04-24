using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using XcssSelectors.Exceptions;
using XcssSelectors.Models;

namespace XcssSelectors.Parsers
{
    internal class XcssParser
    {
        public static List<Models.XcssSelectorData> Parse(string xcssSelector)
        {
            try
            {
                AntlrInputStream inputStream = new AntlrInputStream(xcssSelector);
                AntlrXcssLexer xcssLexer = new AntlrXcssLexer(inputStream);
                xcssLexer.RemoveErrorListeners();
                xcssLexer.AddErrorListener(ThrowingErrorListener.Instance);
                CommonTokenStream commonTokenStream = new CommonTokenStream(xcssLexer);
                AntlrXcssParser xcssParser = new AntlrXcssParser(commonTokenStream);
                xcssParser.RemoveErrorListeners();
                xcssParser.AddErrorListener(ThrowingErrorListener.Instance);
                var listener = new CollectXcssSelectorsListener();
                ParseTreeWalker walker = new ParseTreeWalker();
                walker.Walk(listener, xcssParser.parse());
                return listener.Selectors;
            }
            catch (XcssAntlrParseException ex)
            {
                throw new XcssParseException(xcssSelector, ex.Line, ex.Column, ex.Message, ex);
            }
        }

        private sealed class XcssAntlrParseException : ParseCanceledException
        {
            public int Line { get; }
            public int Column { get; }

            public XcssAntlrParseException(int line, int column, string msg, Exception inner)
                : base($"line {line}:{column} {msg}", inner)
            {
                Line = line;
                Column = column;
            }
        }

        private sealed class ThrowingErrorListener : BaseErrorListener, IAntlrErrorListener<int>
        {
            public static readonly ThrowingErrorListener Instance = new ThrowingErrorListener();

            public override void SyntaxError(
                TextWriter output,
                IRecognizer recognizer,
                IToken offendingSymbol,
                int line,
                int charPositionInLine,
                string msg,
                RecognitionException e
            )
            {
                throw new XcssAntlrParseException(line, charPositionInLine, msg, e);
            }

            public void SyntaxError(
                TextWriter output,
                IRecognizer recognizer,
                int offendingSymbol,
                int line,
                int charPositionInLine,
                string msg,
                RecognitionException e
            )
            {
                throw new XcssAntlrParseException(line, charPositionInLine, msg, e);
            }
        }
    }
}
