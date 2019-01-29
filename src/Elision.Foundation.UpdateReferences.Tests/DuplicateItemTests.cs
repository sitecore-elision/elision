using System.Collections.Specialized;
using FluentAssertions;
using NUnit.Framework;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Web.UI.Sheer;

namespace Elision.Foundation.UpdateReferences.Tests
{
    [TestFixture]
    public class DuplicateItemTests
    {
        [Test]
        public void UpdatesReferencesOnDuplicatedItem()
        {
            var fakeSite = new Sitecore.FakeDb.Sites.FakeSiteContext(
              new Sitecore.Collections.StringDictionary
              {
                  { "name", "website" },
                  { "database", "master" },
                  { "content", "master" },
                  { "rootPath","/sitecore/content"}
              });

            using (new Sitecore.Sites.SiteContextSwitcher(fakeSite))
            using (GetFakeDb())
            {
                var db = Sitecore.Context.Database;
                SetupContextDevice(db);
                var page1 = db.GetItem("/sitecore/content/home/page1");

                var processor = new DuplicateItem(new TreeReferenceUpdater());
                processor.Execute(new ClientPipelineArgs()
                {
                    Parameters = new NameValueCollection()
                    {
                        {"database", "master"},
                        {"id", page1.ID.ToString()},
                        {"name", "page2"}
                    },
                });

                var page2 = db.GetItem("/sitecore/content/home/page2");
                db.GetItem(page2.Fields["Link"].Value).Parent.ID.Should().Be(page2.ID);
            }
        }

        private static void SetupContextDevice(Database db)
        {
            var device = db.GetItem("/sitecore/content/home/device");
            Context.Device = new DeviceItem(device);
        }

        private static Db GetFakeDb()
        {
            var childPage1 = new DbItem("child1");
            var childPage2 = new DbItem("child1");
            var homeId = ID.NewID;
            return new Db
                {
                    new DbItem("home", homeId)
                        {
                            new DbItem("page1")
                                {
                                    new DbField("Link") {Value = childPage1.ID.ToString()},
                                    new DbField("HomePage") {Value = homeId.ToString()},
                                    new DbField(ID.Parse("{E18F4BC6-46A2-4842-898B-B6613733F06F}")) {Value = ""},
                                    new DbField(DeviceFieldIDs.Default) {Value = ""},
                                    childPage1
                                },
                            new DbItem("device", ID.NewID, TemplateIDs.Device)
                        }
                };
        }
    }
}
