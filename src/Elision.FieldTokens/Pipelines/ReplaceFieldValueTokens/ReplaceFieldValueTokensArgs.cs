using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.FieldTokens.Pipelines.ReplaceFieldValueTokens
{
    public class ReplaceFieldValueTokensArgs : PipelineArgs
    {
        public string FieldValue { get; set; }
        public Item Item { get; set; }

        public ReplaceFieldValueTokensArgs(string fieldValue, Item item)
        {
            FieldValue = fieldValue;
            Item = item;
        }
    }
}