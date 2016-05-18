using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.LookupSourceItems.Pipelines.ReplaceLookupSourceQueryTokens
{
    public class ReplaceLookupSourceQueryTokensArgs : PipelineArgs
    {
        public Item ContextItem { get; set; }
        public string Query { get; set; }
        public string TokenPrefix { get; set; }
        public string TokenSuffix { get; set; }

        public ReplaceLookupSourceQueryTokensArgs(Item item, string query, string tokenPrefix = "{", string tokenSuffix = "}")
        {
            ContextItem = item;
            Query = query;
            TokenPrefix = tokenPrefix;
            TokenSuffix = tokenSuffix;
        }
    }
}
