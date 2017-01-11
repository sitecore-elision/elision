using Elision.Foundation.LookupSourceItems.Pipelines.ReplaceLookupSourceQueryTokens;
using Sitecore.Pipelines;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.Foundation.LookupSourceItems.Pipelines.GetLookupSourceItems
{
    public class ReplaceLookupSourceQueryTokens
    {
        public virtual void Process(GetLookupSourceItemsArgs args)
        {
            var replaceTokensArgs = new ReplaceLookupSourceQueryTokensArgs(args.Item,args.Source);
            CorePipeline.Run("elision.replaceLookupSourceQueryTokens", replaceTokensArgs);
            args.Source = replaceTokensArgs.Query;
        }
    }
}
