using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Shell.Framework.Pipelines;

namespace Elision.Foundation.UpdateReferences.Tests
{
    [TestFixture]
    public class CopyOrCloneTests
    {
        [Test]
        public void UpdatesReferencesOnCopiedItem()
        {
            using (GetFakeDb())
            {
                var db = Sitecore.Context.Database;
                SetupContextDevice(db);
                var page1 = db.GetItem("/sitecore/content/home/page1");
                var page2 = db.GetItem("/sitecore/content/home/page2");

                db.GetItem(page2.Fields["Link"].Value).Parent.ID.Should().NotBe(page2.ID);

                var processor = new CopyOrCloneItem(new TreeReferenceUpdater());
                processor.ProcessFieldValues(new CopyItemsArgs()
                {
                    Parameters = new NameValueCollection()
                            {
                              {"database", "master"} ,
                              {"items", page1.Paths.FullPath},
                              {"language", "en"}
                            },
                    Copies = new[] { page2 }
                });

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
                                    childPage1
                                },
                            new DbItem("page2")
                                {
                                    new DbField("Link") {Value = childPage1.ID.ToString()},
                                    new DbField("HomePage") {Value = homeId.ToString()},
                                    childPage2
                                },
                            new DbItem("device", ID.NewID, TemplateIDs.Device)
                        }
                };
        }
    }
}
