using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Data.Items;

namespace Elision.FieldTokens.Pipelines.ReplaceFieldValueTokens
{
    public class ReplaceItemTokens : ReplaceFieldValueTokensProcessorBase
    {
        public override void Process(ReplaceFieldValueTokensArgs args)
        {
            if (args.Item == null)
                return;

            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "DisplayName", args.Item.DisplayName);

            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "Updated", args.Item.Statistics.Updated);
            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "UpdatedBy", args.Item.Statistics.UpdatedBy);

            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "Created", args.Item.Statistics.Created);
            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "CreatedBy", args.Item.Statistics.CreatedBy);

            args.FieldValue = ReplaceFieldValueTokens(args.FieldValue, args.Item);
        }

        protected virtual string ReplaceFieldValueTokens(string value, Item item)
        {
            return Regex.Replace(value, @"{ItemField:(?<fieldName>[^\}:]+)(?<fmt>:[^\}]+)?}", x => string.Format("{0" + x.Groups["fmt"].Value + "}", GetFieldValue(item, x.Groups["fieldName"].Value)), RegexOptions.IgnoreCase);
        }

        protected virtual object GetFieldValue(Item item, string fieldName)
        {
            var field = item?.Fields[fieldName];
            if (field == null)
                return null;

            try
            {
                if (DateUtil.IsIsoDate(field.Value))
                    return DateUtil.IsoDateToDateTime(field.Value);
            }
            catch
            {
            }

            return item[fieldName];
        }
    }
}