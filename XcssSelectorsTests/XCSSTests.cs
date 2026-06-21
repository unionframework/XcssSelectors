using NUnit.Framework;
using XcssSelectors;
using XcssSelectors.Exceptions;

namespace XcssSelectorsTests
{
    //[TestClass]
    //public class ParserTest
    //{
    //    private SpeakParser Setup(string text)
    //    {
    //        AntlrInputStream inputStream = new AntlrInputStream(text);
    //        SpeakLexer speakLexer = new SpeakLexer(inputStream);
    //        CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
    //        SpeakParser speakParser = new SpeakParser(commonTokenStream);

    //        return speakParser;
    //    }

    //    [TestMethod]
    //    public void TestChat()
    //    {
    //        SpeakParser parser = Setup("john says \"hello\" \n michael says \"world\" \n");

    //        SpeakParser.ChatContext context = parser.chat();
    //        BasicSpeakVisitor visitor = new BasicSpeakVisitor();
    //        visitor.Visit(context);

    //        Assert.AreEqual(2, visitor.Lines.Count);
    //    }

    //    [TestMethod]
    //    public void TestLine()
    //    {
    //        SpeakParser parser = Setup("john says \"hello\" \n");

    //        SpeakParser.LineContext context = parser.line();
    //        BasicSpeakVisitor visitor = new BasicSpeakVisitor();
    //        SpeakLine line = (SpeakLine) visitor.VisitLine(context);

    //        Assert.AreEqual("john", line.Person);
    //        Assert.AreEqual("hello", line.Text);
    //    }

    //    [TestMethod]
    //    public void TestWrongLine()
    //    {
    //        SpeakParser parser = Setup("john sayan \"hello\" \n");

    //        var context = parser.line();

    //        Assert.IsInstanceOfType(context, typeof(SpeakParser.LineContext));
    //        Assert.AreEqual("john", context.name().GetText());
    //        Assert.IsNull(context.SAYS());
    //        Assert.AreEqual("johnsayan\"hello\"\n", context.GetText());
    //    }
    //}

    [TestFixture]
    public class XCSSTests
    {
        [TestCase("div[~'text']", "//div[text()[contains(normalize-space(.),'text')]]")]
        [TestCase("div['text']", "//div[text()[normalize-space(.)='text']]")]
        [TestCase(
            "div[src='1.png']['text']",
            "//div[@src='1.png'][text()[normalize-space(.)='text']]"
        )]
        [TestCase(
            "div[src=\"1.png\"]['text']",
            "//div[@src=\"1.png\"][text()[normalize-space(.)='text']]"
        )]
        [TestCase(
            ".classname#myid['text']",
            "//*[@id='myid'][contains(@class,'classname')][text()[normalize-space(.)='text']]"
        )]
        [TestCase(
            ".classname['mytext']",
            "//*[contains(@class,'classname')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div.classname['mytext']",
            "//div[contains(@class,'classname')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            ".classname1.classname2['mytext']",
            "//*[contains(@class,'classname1')][contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div.classname1.classname2['mytext']",
            "//div[contains(@class,'classname1')][contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            ".classname1['mytext'] .classname2['mytext']",
            "//*[contains(@class,'classname1')][text()[normalize-space(.)='mytext']]/descendant::*[contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div.classname1['mytext'] div.classname2['mytext']",
            "//div[contains(@class,'classname1')][text()[normalize-space(.)='mytext']]/descendant::div[contains(@class,'classname2')][text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "#myid div['mytext']",
            "//*[@id='myid']/descendant::div[text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div#myid div['mytext']",
            "//div[@id='myid']/descendant::div[text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div#myid.classname div['mytext']",
            "//div[@id='myid'][contains(@class,'classname')]/descendant::div[text()[normalize-space(.)='mytext']]"
        )]
        [TestCase(
            "div#main-basket-info-div>ul>li['Тариф']>a",
            "//div[@id='main-basket-info-div']/ul/li[text()[normalize-space(.)='Тариф']]/a"
        )]
        [TestCase(
            "li[>h5>strong>a['mytext']]",
            "//li[h5/strong/a[text()[normalize-space(.)='mytext']]]"
        )]
        [TestCase("li[>a]", "//li[a]")]
        [TestCase("li[>a[disabled]]", "//li[a[@disabled]]")]
        [TestCase("tr[1]>td[last()]", "//tr[1]/td[last()]")]
        [TestCase("img[src~'111.png']", "//img[contains(@src,'111.png')]")]
        [TestCase(
            "#showThemesPanel,.genre-filter['text']",
            "//*[@id='showThemesPanel']|//*[contains(@class,'genre-filter')][text()[normalize-space(.)='text']]"
        )]
        [TestCase(
            ">div.toggle-drop>ul>li>span['Вечером']",
            "//child::div[contains(@class,'toggle-drop')]/ul/li/span[text()[normalize-space(.)='Вечером']]"
        )]
        [TestCase("li[10]>div.news-block", "//li[10]/div[contains(@class,'news-block')]")]
        [TestCase(
            "td[h3>span['Категории, на которые вы уже подписаны']]>div>div",
            "//td[descendant::h3/span[text()[normalize-space(.)='Категории, на которые вы уже подписаны']]]/div/div"
        )]
        //[TestCase("tr[span.ng-binding[descendant-or-self::*['{0}']]]", "//tr[descendant::span[contains(@class,'ng-binding')][descendant-or-self::*[normalize-space(text())='{0}'])]]")]
        [TestCase(
            "button[.km-icon.km-email-attachments]+ul",
            "//button[descendant::*[contains(@class,'km-icon')][contains(@class,'km-email-attachments')]]/following-sibling::ul"
        )]
        [TestCase("[data-toggle='collapse'][1]", "//*[@data-toggle='collapse'][1]")]
        [TestCase(
            "input[translate(@type, 'B', 'b')='button']",
            "//input[translate(@type, 'B', 'b')='button']"
        )]
        [TestCase("div>span[not(a)]", "//div/span[not(a)]")]
        [TestCase(
            "div>span[position() mod 2 = 1 and position() > 1]",
            "//div/span[position() mod 2 = 1 and position() > 1]"
        )]
        // Child combinator with surrounding whitespace — regression for combinator trim bug
        [TestCase("div > span", "//div/span")]
        [TestCase(
            ".class1 > .class2",
            "//*[contains(@class,'class1')]/*[contains(@class,'class2')]"
        )]
        [TestCase("tbody > tr[aria-rowindex]", "//tbody/tr[@aria-rowindex]")]
        [TestCase("tbody > tr.data-row > td", "//tbody/tr[contains(@class,'data-row')]/td")]
        // ID selectors with underscores — regression for Name-vs-Ident grammar bug
        [TestCase("#login_wrapper", "//*[@id='login_wrapper']")]
        [TestCase("#log_in", "//*[@id='log_in']")]
        [TestCase("div#main_content", "//div[@id='main_content']")]
        [TestCase("#login_wrapper div", "//*[@id='login_wrapper']/descendant::div")]
        [TestCase(
            "#a_b_c.cls['text']",
            "//*[@id='a_b_c'][contains(@class,'cls')][text()[normalize-space(.)='text']]"
        )]
        public void ConvertXcssOnlyToXpath(string xcssSelector, string result)
        {
            // .Arrange
            // .Act
            var xcss = Xcss.Parse(xcssSelector);

            // .Assert
            Assert.AreEqual(result, xcss.XPath);
        }

        [TestCase("span[data-bind='text: Title']", "//span[@data-bind='text: Title']")]
        [TestCase(
            "#searchPreferences button[type='submit']",
            "//*[@id='searchPreferences']/descendant::button[@type='submit']"
        )]
        //[TestCase("label:contains('Law Firm')", "//label[text()[contains(normalize-space(.),'Law Firm')]]")]
        [TestCase(
            "input~.text-danger",
            "//input/following-sibling::*[contains(@class,'text-danger')]"
        )]
        public void ConvertXcssToXpath(string scssSelector, string result)
        {
            // .Arrange
            // .Act
            var xcss = Xcss.Parse(scssSelector);

            // .Assert
            Assert.AreEqual(result, xcss.XPath);
        }

        [Test]
        public void Concat_SingleSelectors_PopulatesXPathAndSelector()
        {
            var result = Xcss.Parse("div").Concat(Xcss.Parse("a"));
            Assert.That(result.XPath, Is.EqualTo("//div/descendant::a"));
            Assert.That(result.Selector, Is.EqualTo("div a"));
        }

        [Test]
        public void Concat_UnionRoot_ThrowsXcssException()
        {
            var union = Xcss.Parse("div, span");
            var single = Xcss.Parse("a");
            Assert.Throws<XcssException>(() => union.Concat(single));
        }

        [Test]
        public void Concat_UnionRelative_ThrowsXcssException()
        {
            var root = Xcss.Parse("div");
            var union = Xcss.Parse("a, span");
            Assert.Throws<XcssException>(() => root.Concat(union));
        }

        [Test]
        public void Concat_FromXPath_SelectorIsNull()
        {
            var result = Xcss.FromXPath("//div").Concat(Xcss.Parse("a"));
            Assert.That(result.Selector, Is.Null);
            Assert.That(result.XPath, Is.EqualTo("//div/descendant::a"));
        }

        [Test]
        public void Concat_NestedSelectorReparse_PreservesScope()
        {
            // Building a deep selector by re-Parsing each level's .Selector string, Concatenating
            // the next relative part, then exposing .Selector again must keep every ancestor in
            // scope. A regression that dropped the root on Concat collapsed the final XPath to a
            // global leaf match (e.g. //span) instead of staying anchored under .a > [data-row].
            var levelA = Xcss.Parse(".a div[role='grid']");
            var levelB = levelA.Concat(Xcss.Parse("table tr[data-row='0']"));

            var levelBReparsed = Xcss.Parse(levelB.Selector);
            var levelC = levelBReparsed.Concat(Xcss.Parse("span[data-field='x']"));

            var xpath = Xcss.Parse(levelC.Selector).XPath;

            Assert.That(xpath, Does.Contain("contains(@class,'a')"), "must keep the root scope");
            Assert.That(xpath, Does.Contain("tr[@data-row='0']"), "must keep the mid-level scope");
            Assert.That(
                xpath,
                Does.EndWith("/descendant::span[@data-field='x']"),
                "the leaf must hang off the scoped ancestors, not be a global //span"
            );
        }

        [TestCase("#")]
        [TestCase("div[")]
        [TestCase(".foo..bar")]
        public void InvalidSelector_ThrowsParseException(string xcssSelector)
        {
            Assert.Throws<XcssParseException>(() => Xcss.Parse(xcssSelector));
        }

        [TestCase("#", 1, 1)]
        [TestCase("div[", 1, 4)]
        [TestCase(".foo..bar", 1, 5)]
        public void InvalidSelector_XcssParseException_HasPosition(
            string selector,
            int expectedLine,
            int expectedCol
        )
        {
            var ex = Assert.Throws<XcssParseException>(() => Xcss.Parse(selector))!;
            Assert.That(ex.Selector, Is.EqualTo(selector));
            Assert.That(ex.Line, Is.EqualTo(expectedLine));
            Assert.That(ex.Column, Is.EqualTo(expectedCol));
        }

        //[TestCase("#myid", "#myid")]
        //[TestCase("div#myid", "div#myid")]
        //[TestCase("div#myid.classname", "div#myid.classname")]
        //[TestCase(".classname", ".classname")]
        //[TestCase("div.classname", "div.classname")]
        //[TestCase(".classname1.classname2", ".classname1.classname2")]
        //[TestCase("div.classname1.classname2", "div.classname1.classname2")]
        //[TestCase(".classname1 .classname2", ".classname1 .classname2")]
        //[TestCase("div.classname1 div.classname2", "div.classname1 div.classname2")]
        //[TestCase("div[src='1.png']", "div[src='1.png']")]
        //[TestCase("div[src=\"1.png\"]", "div[src=\"1.png\"]")]
        //[TestCase(">.search-bar", ">.search-bar")]
        //[TestCase(".nav-section >.search-bar", ".nav-section >.search-bar")]
        //[TestCase(".nav-section >.search-bar ul", ".nav-section >.search-bar ul")]
        //public void ConvertXcssToCss(string scssSelector, string result)
        //{
        //    var xcss = Xcss.Parse(scssSelector);
        //    Assert.AreEqual(result, xcss.Css);
        //    Assert.IsEmpty(xcss.XPath);
        //}
    }
}
