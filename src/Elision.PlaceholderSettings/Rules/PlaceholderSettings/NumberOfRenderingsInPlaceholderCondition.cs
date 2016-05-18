using System;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;

namespace Elision.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class NumberOfRenderingsInPlaceholderCondition<T> : IntegerComparisonCondition<T> where T : PlaceholderSettingsRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            //var keyRegex = new System.Text.RegularExpressions.Regex(@"\b" + PlaceholderKey + @"(_|$)", RegexOptions.IgnoreCase);

            //if (!keyRegex.IsMatch(ruleContext.FullUniquePlaceholderKey))
            //    return false;

            //return Compare(ruleContext.Args.PlaceholderRenderings.Count);

            var renderings = ruleContext.GetRenderings().ToArray();

            var renderingCount = renderings
                .Count(
                    x =>
                    x.Placeholder.Equals(ruleContext.FullUniquePlaceholderKey,
                                            StringComparison.InvariantCultureIgnoreCase));

            var result = Compare(renderingCount);
            Log.Debug(string.Format("Found {0} matching renderings out of {1} total. Returning comparison result {2}.", renderingCount, renderings.Count(), result));
            return result;
        }
    }
}