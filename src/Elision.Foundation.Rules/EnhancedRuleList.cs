using System.Collections.Generic;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Elision.Foundation.Rules
{
    public class EnhancedRuleList<T> : RuleList<T> where T : EnhancedRuleContext
    {
        public EnhancedRuleList(IEnumerable<Rule<T>> rules) : base(rules)
        {
            Evaluating += EnhancedRuleList_Evaluating;
            Evaluated += EnhancedRuleList_Evaluated;
            Applied += EnhancedRuleList_Applied;
        }

        private void EnhancedRuleList_Evaluated(RuleList<T> ruleList, T ruleContext, Rule<T> rule)
        {
            if (ruleContext.StopProcessingThisRuleset)
                ruleContext.Abort();
        }

        private void EnhancedRuleList_Applied(RuleList<T> ruleList, T ruleContext, RuleAction<T> action)
        {
            if (ruleContext.StopProcessingThisRuleset)
                ruleContext.Abort();
        }

        private void EnhancedRuleList_Evaluating(RuleList<T> ruleList, T ruleContext, Rule<T> rule)
        {
            if (ruleContext.StopProcessingThisRuleset)
                ruleContext.Abort();
        }
    }
}
