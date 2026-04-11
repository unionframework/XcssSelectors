using Antlr4.Runtime.Misc;
using XcssSelectors.Models;

namespace XcssSelectors.Parsers
{

    internal class CollectXcssSelectorsListener : AntlrXcssParserBaseListener
    {
        private Stack<XcssSelectorContext> _parentContexts = new Stack<XcssSelectorContext>();
        private XcssSelectorContext _context;
        public List<XcssSelectorData> Selectors = new List<XcssSelectorData>();

        public override void EnterSelectorGroup([NotNull] AntlrXcssParser.SelectorGroupContext context)
        {
            Selectors = new List<XcssSelectorData>();
            base.EnterSelectorGroup(context);
        }

        public override void EnterSelector([NotNull] AntlrXcssParser.SelectorContext context)
        {
            _context = new XcssSelectorContext(context.GetText());
            base.EnterSelector(context);
        }

        public override void ExitSelector([NotNull] AntlrXcssParser.SelectorContext context)
        {
            if (!_parentContexts.Any())
            {
                // This is a root level selector, not a subelement one
                Selectors.Add(_context.Selector);
            }
            base.ExitSelector(context);
        }

        public override void EnterSubelementSelector([NotNull] AntlrXcssParser.SubelementSelectorContext context)
        {
            _parentContexts.Push(_context);
            base.EnterSubelementSelector(context);
        }

        public override void ExitSubelementSelector([NotNull] AntlrXcssParser.SubelementSelectorContext context)
        {
            var parentContext = _parentContexts.Pop();
            parentContext.Element.SubelementConditions.Add(_context.Selector);
            _context = parentContext;
            base.ExitSubelementSelector(context);
        }


        public override void ExitSimpleSelectorSequence([NotNull] AntlrXcssParser.SimpleSelectorSequenceContext context)
        {
            _context.Selector.Elements.Add(_context.Element);
            _context.Element = new XcssElementData();
            base.ExitSimpleSelectorSequence(context);
        }

        public override void EnterCombinator([NotNull] AntlrXcssParser.CombinatorContext context)
        {
            _context.Element.Combinator = context.GetText();
            base.EnterCombinator(context);
        }

        public override void EnterTagName([NotNull] AntlrXcssParser.TagNameContext context)
        {
            _context.Element.Tag = context.GetText();
            base.EnterTagName(context);
        }

        public override void EnterElementIdValue([NotNull] AntlrXcssParser.ElementIdValueContext context)
        {
            _context.Element.Id = context.GetText();
            base.EnterElementIdValue(context);
        }

        public override void EnterText([NotNull] AntlrXcssParser.TextContext context)
        {
            _context.Text = new XcssTextConditionData();
            base.EnterText(context);
        }

        public override void ExitText([NotNull] AntlrXcssParser.TextContext context)
        {
            _context.Element.TextConditions.Add(_context.Text);
            base.ExitText(context);
        }

        public override void EnterTextMatchStyle([NotNull] AntlrXcssParser.TextMatchStyleContext context)
        {
            switch (context.GetText())
            {
                case "~":
                    _context.Text.MatchStyle = AttributeMatchStyle.Contains;
                    break;
                default:
                    throw new ParseCanceledException("Invalid match style for text.");
            }
            base.EnterTextMatchStyle(context);
        }

        public override void EnterTextValue([NotNull] AntlrXcssParser.TextValueContext context)
        {
            _context.Text.Value = context.GetText();
            base.EnterTextValue(context);
        }

        public override void EnterAttrib([NotNull] AntlrXcssParser.AttribContext context)
        {
            _context.Attribute = new XcssAttributeData();
            base.EnterAttrib(context);
        }

        public override void ExitAttrib([NotNull] AntlrXcssParser.AttribContext context)
        {
            _context.Element.Attributes.Add(_context.Attribute);
            base.ExitAttrib(context);
        }

        public override void EnterAttribName([NotNull] AntlrXcssParser.AttribNameContext context)
        {
            _context.Attribute.Name = context.GetText();
            base.EnterAttribName(context);
        }

        public override void EnterAttribMatchStyle([NotNull] AntlrXcssParser.AttribMatchStyleContext context)
        {
            switch (context.GetText())
            {
                case "^=":
                    _context.Attribute.MatchStyle = AttributeMatchStyle.Prefix;
                    break;
                case "$=":
                    _context.Attribute.MatchStyle = AttributeMatchStyle.Suffix;
                    break;
                case "*=":
                case "~=":
                case "~":
                    _context.Attribute.MatchStyle = AttributeMatchStyle.Contains;
                    break;
                case "=":
                    _context.Attribute.MatchStyle = AttributeMatchStyle.Equal;
                    break;
                default:
                    throw new ParseCanceledException("Invalid match style for attribute.");
            }
            base.EnterAttribMatchStyle(context);
        }

        public override void EnterAttribValue([NotNull] AntlrXcssParser.AttribValueContext context)
        {
            _context.Attribute.Value = context.GetText();
            base.EnterAttribValue(context);
        }

        public override void EnterClassNameValue([NotNull] AntlrXcssParser.ClassNameValueContext context)
        {
            _context.Element.ClassNames.Add(context.GetText());
            base.EnterClassNameValue(context);
        }

        public override void EnterPseudo([NotNull] AntlrXcssParser.PseudoContext context)
        {
            _context.Element.Conditions.Add(context.GetText());
            base.EnterPseudo(context);
        }

        public override void EnterNegation([NotNull] AntlrXcssParser.NegationContext context)
        {
            throw new NotImplementedException();
            //base.EnterNegation(context);
        }

        public override void EnterElementIndex([NotNull] AntlrXcssParser.ElementIndexContext context)
        {
            if (_context.Element.Index != null)
            {
                throw new ParseCanceledException("Element can not contain several indexes.");
            }
            _context.Element.Index = int.Parse(context.GetText());
            base.EnterElementIndex(context);
        }

        public override void EnterXpathCondition([NotNull] AntlrXcssParser.XpathConditionContext context)
        {
            _context.StartXpathCondition(context.GetText());
            base.EnterXpathCondition(context);
        }

        public override void ExitXpathCondition([NotNull] AntlrXcssParser.XpathConditionContext context)
        {
            _context.FinishXpathCondition();
            base.ExitXpathCondition(context);
        }

    }
}