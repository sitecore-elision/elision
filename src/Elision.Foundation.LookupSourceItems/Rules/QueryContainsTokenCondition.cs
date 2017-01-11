using Sitecore.Rules.Conditions;

namespace Elision.Foundation.LookupSourceItems.Rules
{
    public class QueryContainsTokenCondition<T> : WhenCondition<T> where T : GetLookupsourceItemsRuleContext
    {
        public string Token { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (string.IsNullOrWhiteSpace(ruleContext?.Args?.Source))
                return false;

            return ruleContext.Args.Source.Contains(string.Concat("{", Token, "}"))
                || ruleContext.Args.Source.Contains(string.Concat("{", Token, ":"));
        }
    }
}