using Elision.Foundation.LookupSourceItems.Rules;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Pipelines.GetLookupSourceItems;
using Sitecore.Rules;

namespace Elision.Foundation.LookupSourceItems.Tests
{
    [TestFixture]
    public class QueryContainsTokenConditionTests
    {
        [Test]
        public void DetectsMatchingToken()
        {
            using (var db = new Db() { new DbItem("page1"), new DbItem("page2") })
            {
                var page1 = db.GetItem("/sitecore/content/page1");

                var args = new GetLookupSourceItemsArgs() { Item = page1, Source = "{Home}/page1" };

                var context = new GetLookupsourceItemsRuleContext(args);
                var condition = new QueryContainsTokenCondition<GetLookupsourceItemsRuleContext>()
                {
                    Token = "Home"
                };

                var ruleStack = new RuleStack();

                condition.Evaluate(context, ruleStack);

                ruleStack.Should().HaveCount(1);
                ruleStack.Pop().Should().Be(true);
            }
        }

        [Test]
        public void DetectsItemFieldToken()
        {
            using (var db = new Db() { new DbItem("page1"), new DbItem("page2") })
            {
                var page1 = db.GetItem("/sitecore/content/page1");

                var args = new GetLookupSourceItemsArgs() { Item = page1, Source = "{ItemField:FullName}" };

                var context = new GetLookupsourceItemsRuleContext(args);
                var condition = new QueryContainsTokenCondition<GetLookupsourceItemsRuleContext>()
                {
                    Token = "ItemField:FullName"
                };

                var ruleStack = new RuleStack();

                condition.Evaluate(context, ruleStack);

                ruleStack.Should().HaveCount(1);
                ruleStack.Pop().Should().Be(true);
            }
        }

        [Test]
        public void DetectsItemFieldTokenWithoutFieldName()
        {
            using (var db = new Db() { new DbItem("page1"), new DbItem("page2") })
            {
                var page1 = db.GetItem("/sitecore/content/page1");

                var args = new GetLookupSourceItemsArgs() { Item = page1, Source = "{ItemField:FullName}" };

                var context = new GetLookupsourceItemsRuleContext(args);
                var condition = new QueryContainsTokenCondition<GetLookupsourceItemsRuleContext>()
                {
                    Token = "ItemField"
                };

                var ruleStack = new RuleStack();

                condition.Evaluate(context, ruleStack);

                ruleStack.Should().HaveCount(1);
                ruleStack.Pop().Should().Be(true);
            }
        }

        [Test]
        public void ReturnsFalseForNullQuery()
        {
            using (var db = new Db() { new DbItem("page1"), new DbItem("page2") })
            {
                var page1 = db.GetItem("/sitecore/content/page1");

                var args = new GetLookupSourceItemsArgs() { Item = page1 };

                var context = new GetLookupsourceItemsRuleContext(args);
                var condition = new QueryContainsTokenCondition<GetLookupsourceItemsRuleContext>()
                {
                    Token = "ItemField"
                };

                var ruleStack = new RuleStack();

                condition.Evaluate(context, ruleStack);

                ruleStack.Should().HaveCount(1);
                ruleStack.Pop().Should().Be(false);
            }
        }
    }
}
