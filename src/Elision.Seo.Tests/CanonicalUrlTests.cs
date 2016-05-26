using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Elision.Seo.Tests
{
    [TestFixture]
    public class CanonicalUrlTests
    {
        [Test]
        public void LowerCasesRawUrl()
        {
            var rawUrl = "/Page Url";
            var expectedResult = "/page url";

            using (GetFakeDb())
            {
                ((Item) null).GetCanonicalUrl(rawUrl)
                    .Should().Be(expectedResult);
            }
        }

        [Test]
        public void GetsCorrectAliasedCanonical()
        {
            var rawUrl = "/aliased";
            var resolvedUrl = "/page";

            using (GetFakeDb())
            {
                ((Item)null).GetCanonicalUrl(rawUrl)
                    .Replace(".aspx", "").Should().EndWith(resolvedUrl);
            }
        }

        [Test]
        public void EmptyForUnchangedUrls()
        {
            var rawUrl = "/page-url";

            using (GetFakeDb())
            {
                ((Item)null).GetCanonicalUrl(rawUrl)
                    .Should().BeNull();
            }
        }

        private static Db GetFakeDb()
        {
            var pageId = new ID();
            var pageItem = new DbItem("page", pageId);
            var aliases = new DbItem("Aliases")
                {
                    new DbItem("aliased")
                        {
                            new DbLinkField("Linked item") { LinkType = "internal", TargetID = pageId }
                        }
                };
            aliases.ParentID = Sitecore.ItemIDs.SystemRoot;

            var db = new Db()
                {
                    aliases,
                    pageItem
                };
            return db;
        }
    }
}
