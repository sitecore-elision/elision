using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Elision.Foundation.UpdateReferences.Tests
{
    [TestFixture]
    public class ReferenceUpdaterTests
    {
        [Test]
        public void UpdatesLinkFieldValue()
        {
            using (GetFakeDb())
            {
                var db = Sitecore.Context.Database;
                var page1 = db.GetItem("/sitecore/content/home/page1");
                var page2 = db.GetItem("/sitecore/content/home/page2");

                db.GetItem(page2.Fields["Link"].Value).Parent.ID.Should().NotBe(page2.ID);

                var updater = new TreeReferenceUpdater();
                updater.UpdateReferences(page1, page2);

                db.GetItem(page2.Fields["Link"].Value).Parent.ID.Should().Be(page2.ID);
            }
        }

        [Test]
        public void UpdatesMultilistFieldValue()
        {
            using (GetFakeDb())
            {
                var db = Sitecore.Context.Database;
                var page1 = db.GetItem("/sitecore/content/home/page1");
                var page2 = db.GetItem("/sitecore/content/home/page2");

                var child1 = page1.Children.First(x => x.Name == "child");
                var child2 = page2.Children.First(x => x.Name == "child");

                page2.Fields["MultiLink"].Value.Should().Contain(child1.ID.ToString())
                                         .And.NotContain(child2.ID.ToString());

                var updater = new TreeReferenceUpdater();
                updater.UpdateReferences(page1, page2);

                page2.Fields["MultiLink"].Value.Should().Contain(child2.ID.ToString())
                                         .And.NotContain(child1.ID.ToString());
            }
        }

        private static Db GetFakeDb()
        {
            var childPage1 = new DbItem("child");
            var childPage2 = new DbItem("child");
            var homeId = ID.NewID;
            return new Db
                {
                    new DbItem("home", homeId)
                        {
                            new DbItem("page1")
                                {
                                    new DbField("Link") {Value = childPage1.ID.ToString()},
                                    new DbField("MultiLink"){Value = homeId + "|" + childPage1.ID},
                                    childPage1
                                },
                            new DbItem("page2")
                                {
                                    new DbField("Link") {Value = childPage1.ID.ToString()},
                                    new DbField("MultiLink"){Value = homeId + "|" + childPage1.ID},
                                    childPage2
                                }
                        }
                };
        }
    }
}
