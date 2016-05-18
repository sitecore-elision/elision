using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Actions;

namespace Elision.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class ApplyPlaceholderSettingsAction<T> : RuleAction<T> where T : PlaceholderSettingsRuleContext
    {
        public string PlaceholderSettingsId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (string.IsNullOrWhiteSpace(PlaceholderSettingsId)) return;

            var allSettings = PlaceholderSettingsId
                .Split('|')
                .Where(ID.IsID)
                .Select(ID.Parse)
                .Distinct()
                .Select(ruleContext.Args.ContentDatabase.GetItem)
                .Where(x => x != null);

            var renderings = allSettings
                .Select(x => x.Fields["Allowed Controls"])
                .Where(x => x != null)
                .SelectMany(x => (x.Value ?? "").Split('|'))
                .Where(ID.IsID)
                .Select(ID.Parse)
                .Distinct()
                .Select(ruleContext.Args.ContentDatabase.GetItem)
                .Where(x => x != null);

            foreach (var rendering in renderings.Where(r => ruleContext.Args.PlaceholderRenderings.All(x => x.ID != r.ID)))
            {
                ruleContext.Args.PlaceholderRenderings.Add(rendering);
            }
        }
    }
}
