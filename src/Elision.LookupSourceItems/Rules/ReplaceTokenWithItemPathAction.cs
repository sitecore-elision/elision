using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.LookupSourceItems.Rules
{
    public class ReplaceTokenWithItemPathAction<T> : RuleAction<T> where T : GetLookupsourceItemsRuleContext
    {
        public string NewItemId { get; set; }
        public string Token { get; set; }

        public override void Apply(T ruleContext)
        {
            if (!ID.IsID(NewItemId))
                return;

            if (string.IsNullOrWhiteSpace(Token))
                return;

            var newItem = ruleContext.Item.Database.GetItem(ID.Parse(NewItemId));
            if (newItem == null)
                return;

            ruleContext.Args.Source = ruleContext.Args.Source.Replace(string.Concat("{", Token, "}"), newItem.Paths.FullPath);
        }
    }
}
