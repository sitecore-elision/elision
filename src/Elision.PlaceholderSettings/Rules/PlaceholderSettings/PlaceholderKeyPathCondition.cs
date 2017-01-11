using Sitecore.Rules.Conditions;

namespace Elision.Foundation.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class PlaceholderKeyPathCondition<T> : StringOperatorCondition<T> where T : PlaceholderSettingsRuleContext
    {
        public string Value { get; set; }

        protected override bool Execute(T ruleContext)
        {
            return Compare(ruleContext.PlaceholderKeyPath, Value);
        }
    }
}