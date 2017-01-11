using Elision.Foundation.LookupSourceItems.Rules;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.Foundation.LookupSourceItems.Tests
{
    [TestFixture]
    public class SetQueryContextItemActionTests
    {
        [Test]
        public void SetsContextItem()
        {
            using (var db = new Db()
                {
                    new DbItem("page1"),
                    new DbItem("page2")
                })
            {
                var page1 = db.GetItem("/sitecore/content/page1");
                var page2 = db.GetItem("/sitecore/content/page2");

                var args = new GetLookupSourceItemsArgs() {Item = page1, Source = "./*"};

                var ctx = new GetLookupsourceItemsRuleContext(args);
                var action = new SetQueryContextItemAction<GetLookupsourceItemsRuleContext>()
                    {
                        NewContextItemId = page2.ID.ToString()
                    };

                action.Apply(ctx);

                ctx.Args.Item.ID.Should().Be(page2.ID);
            }
        }
    }
}
