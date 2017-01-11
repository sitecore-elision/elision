using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.Foundation.LookupSourceItems.Rules
{
    public class SetQueryContextItemAction<T> : RuleAction<T> where T : GetLookupsourceItemsRuleContext
    {
        public string NewContextItemId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (!ID.IsID(NewContextItemId))
                return;

            var newItem = ruleContext.Item.Database.GetItem(ID.Parse(NewContextItemId));
            if (newItem == null)
                return;

            ruleContext.Args.Item = newItem;
        }
    }
}
