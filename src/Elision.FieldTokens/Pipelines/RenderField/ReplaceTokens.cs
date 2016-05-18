using Elision.FieldTokens.Pipelines.ReplaceFieldValueTokens;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Sites;
using Sitecore.Web;

namespace Elision.FieldTokens.Pipelines.RenderField
{
    public class ReplaceTokens
    {
        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (!ShouldReplaceTokens(args)) return;

            var replaceTokensArgs = new ReplaceFieldValueTokensArgs(args.Result.FirstPart, args.Item);

            CorePipeline.Run("elision.replaceFieldValueTokens", replaceTokensArgs);

            args.Result.FirstPart = replaceTokensArgs.FieldValue;
        }

        private bool ShouldReplaceTokens(RenderFieldArgs args)
        {
            return args.Item != null && (!CanWebEdit(args) || !CanEditItem(args.Item));
        }

        private bool CanWebEdit(RenderFieldArgs args)
        {
            if (args.DisableWebEdit)
                return false;
            var site = Context.Site;
            return site != null && site.DisplayMode == DisplayMode.Edit && (WebUtil.GetQueryString("sc_duration") != "temporary" && Context.PageMode.IsExperienceEditorEditing);
        }

        private bool CanEditItem(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return (Context.IsAdministrator || !item.Locking.IsLocked() || item.Locking.HasLock()) && (item.Access.CanWrite() && item.Access.CanWriteLanguage() && !item.Appearance.ReadOnly);
        }
    }
}