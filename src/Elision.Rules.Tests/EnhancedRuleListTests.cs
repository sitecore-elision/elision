using Elision.Rules.RuleProcessing;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Elision.Rules.Tests
{
    [TestFixture]
    public class EnhancedRuleListTests
    {
        [Test]
        public void AbortsWhenToldToStopProcessingRuleset()
        {
            var rules = new[]
            {
                new Rule<EnhancedRuleContext>(new TrueCondition<EnhancedRuleContext>(), new StopProcessingThisRulesetAction<EnhancedRuleContext>())
            };
            var list = new EnhancedRuleList<EnhancedRuleContext>(rules);
            var context = new EnhancedRuleContext();

            list.Run(context);

            context.IsAborted.Should().BeTrue();
        }
    }
}
