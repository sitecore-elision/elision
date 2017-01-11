using Sitecore.Rules;

namespace Elision.Foundation.Rules
{
    public class EnhancedRuleContext : RuleContext
    {
        public bool StopProcessingThisRuleset { get; set; }
        public bool StopProcessingAfterThisRuleset { get; set; }
    }
}
