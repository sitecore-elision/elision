using System;
using Elision.LookupSourceItems.Rules;
using Elision.Rules;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.LookupSourceItems.Pipelines.GetLookupSourceItems
{
    public class RunGetLookupSourceItemsRules
    {
        private readonly IRulesRunner _runner;

        public RunGetLookupSourceItemsRules(IRulesRunner runner)
        {
            _runner = runner;
        }

        public void Process(GetLookupSourceItemsArgs args)
        {
            try
            {
                _runner.RunGlobalRules("Get Lookup Source Items",
                                        args.Item.Database,
                                        new GetLookupsourceItemsRuleContext(args));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }
    }
}
