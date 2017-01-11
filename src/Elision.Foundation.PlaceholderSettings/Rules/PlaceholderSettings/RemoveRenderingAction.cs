using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.Foundation.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class RemoveRenderingAction<T> : RuleAction<T> where T : PlaceholderSettingsRuleContext
    {
        public string RenderingItemIds { get; set; }

        public override void Apply(T ruleContext)
        {
            var allIds = RenderingItemIds
                .Split('|')
                .Where(ID.IsID)
                .Select(ID.Parse);

            ruleContext.Args.PlaceholderRenderings.RemoveAll(x => allIds.Contains(x.ID));
        }
    }
}
