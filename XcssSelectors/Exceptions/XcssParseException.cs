namespace XcssSelectors.Exceptions
{
    public class XcssParseException : Exception
    {
        public string Selector { get; }
        public int Line { get; }
        public int Column { get; }

        internal XcssParseException(string selector, int line, int column, string msg, Exception inner)
            : base($"Failed to parse selector '{selector}' at {line}:{column}: {msg}", inner)
        {
            Selector = selector;
            Line = line;
            Column = column;
        }
    }
}
