using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.Foundation.CompatibleRenderings.Rules.CompatibleRenderings
{
    public class AddCompatibleRenderingAction<T> : RuleAction<T> where T : GetCompatibleRenderingsRuleContext
    {
        public string RenderingIds { get; set; }

        public override void Apply(T ruleContext)
        {
            if (string.IsNullOrWhiteSpace(RenderingIds))
                return;

            foreach (var id in RenderingIds.Split('|').Where(ID.IsID).Select(ID.Parse))
            {
                if (!ruleContext.CompatibleRenderings.Contains(id))
                    ruleContext.CompatibleRenderings.Add(id);
            }
        }
    }
}
