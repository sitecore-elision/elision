using Elision.Foundation.Rules;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.Foundation.LookupSourceItems.Rules
{
    public class GetLookupsourceItemsRuleContext : EnhancedRuleContext
    {
        public GetLookupSourceItemsArgs Args { get; set; }

        public GetLookupsourceItemsRuleContext(GetLookupSourceItemsArgs args)
        {
            Args = args;
            Item = args.Item;
        }
    }
}