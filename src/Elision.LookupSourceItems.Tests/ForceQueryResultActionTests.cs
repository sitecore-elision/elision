using Elision.LookupSourceItems.Rules;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.LookupSourceItems.Tests
{
    [TestFixture]
    public class ForceQueryResultActionTests
    {
        [Test]
        public void SetsResult()
        {
            using (var db = GetFakeDb())
            {
                var page = db.GetItem("/sitecore/content/page");

                var context = new GetLookupsourceItemsRuleContext(new GetLookupSourceItemsArgs()
                {
                    Item = page
                });

                var action = new ForceQueryResultAction<GetLookupsourceItemsRuleContext>()
                {
                    ResultItemId = page.ID.ToString()
                };
                action.Apply(context);

                context.Args.Result.Count.Should().Be(1);
                context.Args.Result[0].ID.Should().Be(page.ID);
            }
        }

        [Test]
        public void OverridesExistingResults()
        {
            using (var db = GetFakeDb())
            {
                var page = db.GetItem("/sitecore/content/page");
                var page2 = db.GetItem("/sitecore/content/page2");

                var context = new GetLookupsourceItemsRuleContext(new GetLookupSourceItemsArgs()
                {
                    Item = page,
                    Result = {page2}
                });

                var action = new ForceQueryResultAction<GetLookupsourceItemsRuleContext>()
                {
                    ResultItemId = page.ID.ToString()
                };
                action.Apply(context);

                context.Args.Result.Count.Should().Be(1);
                context.Args.Result[0].ID.Should().Be(page.ID);
            }
        }

        [Test]
        public void AbortsPipeline()
        {
            using (var db = GetFakeDb())
            {
                var page = db.GetItem("/sitecore/content/page");
                var page2 = db.GetItem("/sitecore/content/page2");

                var context = new GetLookupsourceItemsRuleContext(new GetLookupSourceItemsArgs()
                {
                    Item = page,
                    Result = { page2 }
                });

                var action = new ForceQueryResultAction<GetLookupsourceItemsRuleContext>()
                {
                    ResultItemId = page.ID.ToString()
                };
                action.Apply(context);

                context.Args.Aborted.Should().BeTrue();
            }
        }

        [Test]
        public void AbortsRulesExecution()
        {
            using (var db = GetFakeDb())
            {
                var page = db.GetItem("/sitecore/content/page");
                var page2 = db.GetItem("/sitecore/content/page2");

                var context = new GetLookupsourceItemsRuleContext(new GetLookupSourceItemsArgs()
                {
                    Item = page,
                    Result = { page2 }
                });

                var action = new ForceQueryResultAction<GetLookupsourceItemsRuleContext>()
                {
                    ResultItemId = page.ID.ToString()
                };
                action.Apply(context);

                context.IsAborted.Should().BeTrue();
            }
        }

        private Db GetFakeDb()
        {
            return new Db
            {
                new DbItem("page"),
                new DbItem("page2"),
                new DbItem("page3")
            };
        }
    }
}
