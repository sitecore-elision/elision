using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Conditions;

namespace Elision.Foundation.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class NumberOfSpecificRenderingOnPageCondition<T> : IntegerComparisonCondition<T> where T : PlaceholderSettingsRuleContext
    {
        public string RenderingItemId { get; set; }

        protected override bool Execute(T ruleContext)
        {
            var allIds = (RenderingItemId ?? "")
                .Split('|')
                .Where(ID.IsID)
                .Select(ID.Parse)
                .ToArray();

            var renderings = ruleContext.GetRenderings().ToArray();

            var renderingCount = renderings.Count(x => allIds.Contains(x.RenderingID));
            return Compare(renderingCount);
        }
    }
}