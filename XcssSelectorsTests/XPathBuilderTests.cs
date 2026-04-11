using NUnit.Framework;
using XcssSelectors.Builders;

namespace XcssSelectorsTests
{
    [TestFixture]
    public class XpathBuilderTest
    {
        //*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']

        [TestCase("div", true)]
        [TestCase("//div", true)]
        [TestCase("//div[@id='myId']", true)]
        [TestCase("//div[text()='mytext']", true)]
        [TestCase("//div[text()='mytext' and @class='myclass']", true)]
        [TestCase("//div[@id='myId']/descendant::span", true)]
        [TestCase("//div[@id='myId1']|//div[@id='myId2']", true)]
        [TestCase("#myId", false)]
        [TestCase(".myclass", false)]
        public void IsXpath(string xpath, bool isXpath)
        {
            Assert.AreEqual(isXpath, XPathBuilder.IsXPath(xpath));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void RootIsEmpty(string root)
        {
            var relative = "div";
            Assert.AreEqual("//div", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void ConcatAsDescendant()
        {
            var root = "//div";
            var relative = "*[@id='myid']";
            Assert.AreEqual("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void InsertArgsToRelative()
        {
            var root = "//div";
            var relative = "*[@id='{0}']";
            Assert.AreEqual("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));

            root = "//div[@id='{0}']";
            relative = "*[@id='{0}']";
            Assert.AreEqual("//div[@id='{0}']/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));
        }

        [Test]
        public void LeaveAxis()
        {
            var root = "//div";
            var relative = "self::*[@id='myid']";
            Assert.AreEqual("//div/self::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void MakeRelative()
        {
            var root = "//*[@id='aaa1']";
            var relative = "//*[@id='bbb']";
            Assert.AreEqual("//*[@id='aaa1']/descendant::*[@id='bbb']", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void MultipleRootXpath()
        {
            var root = "//*[@id='aaa1'] | //*[@id='aaa2']";
            var relative = "*[@id='bbb']";
            Assert.AreEqual(
                "//*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']",
                XPathBuilder.Concat(root, relative, "myid"));
        }

        [Test]
        public void RelativeIsEmpty()
        {
            var root = "//*[@id='aaa1']";
            var relative = "";
            Assert.AreEqual("//*[@id='aaa1']", XPathBuilder.Concat(root, relative));
        }
    }
}
