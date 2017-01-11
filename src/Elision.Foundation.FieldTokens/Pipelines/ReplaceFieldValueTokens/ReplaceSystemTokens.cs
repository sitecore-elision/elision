using System;

namespace Elision.Foundation.FieldTokens.Pipelines.ReplaceFieldValueTokens
{
    public class ReplaceSystemTokens : ReplaceFieldValueTokensProcessorBase
    {
        public override void Process(ReplaceFieldValueTokensArgs args)
        {
            DateTime dateToUse;
            try
            {
                dateToUse = Sitecore.Configuration.State.Previewing
                    ? Sitecore.Configuration.State.PreviewDate
                    : DateTime.Now;
            }
            catch
            {
                dateToUse = DateTime.Now;
            }
            args.FieldValue = ReplaceTokenWithOptionalFormat(args.FieldValue, "Now", dateToUse);
        }
    }
}