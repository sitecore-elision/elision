using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class AddRenderingAction<T> : RuleAction<T> where T : PlaceholderSettingsRuleContext
    {
        public string RenderingItemIds { get; set; }

        public override void Apply(T ruleContext)
        {
            var allIds = RenderingItemIds
                .Split('|')
                .Where(ID.IsID)
                .Select(ID.Parse);
            var renderings = allIds
                .Select(ruleContext.Args.ContentDatabase.GetItem)
                .Where(x => x != null);

            foreach (var rendering in renderings.Where(r => ruleContext.Args.PlaceholderRenderings.All(x => x.ID != r.ID)))
            {
                ruleContext.Args.PlaceholderRenderings.Add(rendering);
            }
        }
    }
}
