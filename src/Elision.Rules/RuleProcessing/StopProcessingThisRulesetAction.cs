using Sitecore.Rules.Actions;

namespace Elision.Foundation.Rules.RuleProcessing
{
    public class StopProcessingThisRulesetAction<T> : RuleAction<T> where T : EnhancedRuleContext
    {
        public override void Apply(T ruleContext)
        {
            ruleContext.StopProcessingThisRuleset = true;
        }
    }
}
