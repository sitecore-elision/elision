using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.LookupSourceItems.Rules
{
    public class ForceQueryResultAction<T> : RuleAction<T> where T : GetLookupsourceItemsRuleContext
    {
        public string ResultItemId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (!ID.IsID(ResultItemId))
                return;

            var newItem = ruleContext.Item.Database.GetItem(ID.Parse(ResultItemId));
            if (newItem == null)
                return;

            ruleContext.Args.Result.Clear();
            ruleContext.Args.Result.Add(newItem);

            ruleContext.Args.AbortPipeline();
            ruleContext.Abort();
        }
    }
}
