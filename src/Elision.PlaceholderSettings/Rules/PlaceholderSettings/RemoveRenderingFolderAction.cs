using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class RemoveRenderingFolderAction<T> : RuleAction<T> where T : PlaceholderSettingsRuleContext
    {
        public string RenderingFolderItemId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (!ID.IsID(RenderingFolderItemId)) return;

            var folder = ruleContext.Args.ContentDatabase.GetItem(RenderingFolderItemId);
            if (folder == null) return;

            ruleContext.Args.PlaceholderRenderings.RemoveAll(x => x.Axes.IsDescendantOf(folder));
        }
    }
}
