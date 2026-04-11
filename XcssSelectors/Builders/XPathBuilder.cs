namespace XcssSelectors.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.XPath;
    using XcssSelectors.Models;

    public class XPathBuilder
    {
        public const string DESCENDANT_AXIS = "descendant::";

        public const string ANCESTOR_AXIS = "ancestor::";

        public const string ANCESTORORSELF_AXIS = "ancestor-or-self::";

        public const string ATTRIBUTE_AXIS = "attribute::";

        public const string CHILD_AXIS = "child::";

        public const string DESCENDANTORSELF_AXIS = "descendant-or-self::";

        public const string FOLLOWING_AXIS = "following::";

        private const string FOLLOWING_SIBLING_AXIS = "following-sibling::";

        public const string NAMESPACE_AXIS = "namespace::";

        public const string PARENT_AXIS = "parent::";

        public const string PRECEDING_AXIS = "preceding::";

        public const string PRECEDINGSIBLING_AXIS = "preceding-sibling::";

        public const string SELF_AXIS = "self::";

        private const string XPATH_ROOT = "//";

        public static string Concat(string root, string relative, params object[] args)
        {
            relative = string.Format(relative, args);
            if (relative.StartsWith(XPATH_ROOT))
            {
                relative = relative.Substring(2, relative.Length - 2);
            }
            if (string.IsNullOrWhiteSpace(relative))
            {
                if (string.IsNullOrWhiteSpace(root))
                {
                    throw new Exception("Invalid xpath: root and relative parts are empty");
                }
                return root;
            }
            if (string.IsNullOrWhiteSpace(root))
            {
                return XPATH_ROOT + relative;
            }
            var rootXpaths = root.Split('|');
            if (rootXpaths.Length == 1)
            {
                var axis = HasAxis(relative) ? string.Empty : DESCENDANT_AXIS;
                return string.Format("{0}/{1}{2}", root, axis, relative);
            }
            var s = rootXpaths.Aggregate(
                string.Empty,
                (current, rootXpath) => current + Concat(rootXpath.Trim(), relative) + "|");
            return s.Substring(0, s.Length - 1);
        }

        private static bool HasAxis(string xpath)
        {
            var axises = new List<string>
                             {
                                 ANCESTOR_AXIS,
                                 ANCESTORORSELF_AXIS,
                                 ATTRIBUTE_AXIS,
                                 CHILD_AXIS,
                                 DESCENDANT_AXIS,
                                 DESCENDANTORSELF_AXIS,
                                 FOLLOWING_AXIS,
                                 FOLLOWING_SIBLING_AXIS,
                                 NAMESPACE_AXIS,
                                 PARENT_AXIS,
                                 PRECEDING_AXIS,
                                 PRECEDINGSIBLING_AXIS,
                                 SELF_AXIS
                             };
            return axises.Any(xpath.StartsWith);
        }

        public static bool IsXPath(string value)
        {
            try
            {
                XPathExpression.Compile(value);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        internal static string Build(XcssElementData selector)
        {
            var tag = string.IsNullOrEmpty(selector.Tag) ? "*" : selector.Tag;
            var xpath = XpathAxis(selector.Combinator) + tag;
            if (!string.IsNullOrEmpty(selector.Id))
            {
                xpath += XpathAttributeCondition("id", string.Format("'{0}'", selector.Id));
            }
            foreach (var className in selector.ClassNames)
            {
                xpath += XpathAttributeCondition("class", string.Format("'{0}'", className),
                    AttributeMatchStyle.Contains);
            }
            foreach (var attribute in selector.Attributes)
            {
                xpath += XpathAttributeCondition(attribute.Name, attribute.Value, attribute.MatchStyle);
            }
            foreach (var condition in selector.TextConditions)
            {
                xpath += XpathTextCondition(condition);
            }
            foreach (var subelementCondition in selector.SubelementConditions)
            {
                xpath += XpathCondition(RemoveChildAxis(Build(subelementCondition)));
            }
            foreach (var subelementXpath in selector.SubelementXPathConditions)
            {
                xpath += XpathCondition(subelementXpath);
            }
            if (selector.Index.HasValue)
            {
                xpath += XpathCondition(selector.Index.ToString());
            }
            return xpath;
        }

        internal static string Build(XcssSelectorData selector)
        {
            var xpath = string.Empty;
            for (var i = 0; i < selector.Elements.Count; i++)
            {
                var elementXpath = Build(selector.Elements[i]);
                if (i > 0)
                {
                    elementXpath = "/" + RemoveChildAxis(elementXpath);
                }
                xpath += elementXpath;
            }
            return xpath;
        }

        private static string RemoveChildAxis(string elementXpath)
        {
            if (elementXpath.StartsWith(CHILD_AXIS))
                elementXpath = elementXpath.Remove(0, CHILD_AXIS.Length);
            return elementXpath;
        }

        internal static IEnumerable<string> Build(List<XcssSelectorData> selectors, XcssOptions options)
        {
            return selectors.Select(s => "//" + RemoveDescendantAxis(Build(s)));
        }

        internal static string Combine(IEnumerable<string> xpaths)
        {
            return string.Join('|', xpaths);
        }

        private static string RemoveDescendantAxis(string elementXpath)
        {
            if (elementXpath.StartsWith(DESCENDANT_AXIS))
                elementXpath = elementXpath.Remove(0, DESCENDANT_AXIS.Length);
            return elementXpath;
        }

        private static string XpathCondition(string condition) => $"[{condition}]";

        private static string XpathAxis(string axis)
        {
            switch (axis)
            {
                case null:
                case "":
                case " ":
                    return DESCENDANT_AXIS;
                case ">":
                    return CHILD_AXIS;
                case "+":
                case "~":
                    return FOLLOWING_SIBLING_AXIS;
                default:
                    throw new ArgumentOutOfRangeException("axis");
            }
        }

        private static string XpathAttributeCondition(string name, string value, AttributeMatchStyle style = AttributeMatchStyle.Equal)
        {
            if (value == null)
            {
                return $"[@{name}]";
            }
            switch (style)
            {
                case AttributeMatchStyle.Equal:
                    return string.Format("[@{0}={1}]", name, value);
                case AttributeMatchStyle.Contains:
                    return string.Format("[contains(@{0},{1})]", name, value);
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
        }

        private static string XpathTextCondition(XcssTextConditionData textCondition)
        {
            if (textCondition.MatchStyle == AttributeMatchStyle.Contains)
            {
                return $"[text()[contains(normalize-space(.),{textCondition.Value})]]";
            }

            return $"[text()[normalize-space(.)={textCondition.Value}]]";
        }
    }

}