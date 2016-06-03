using Sitecore.Buckets.Rules.Bucketing;
using Sitecore.Buckets.Rules.Bucketing.Conditions;

namespace Elision.Rules.Bucketing
{
    public class WhenNewItemTemplateInheritsFrom<T> : WhenNewItemTemplateIs<T> where T : BucketingRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            var result = base.Execute(ruleContext);
            if (result) return true;

            var template = ruleContext.Database.GetTemplate(ruleContext.NewItemTemplateId);
            if (template == null) return false;

            return template.InheritsFrom(TemplateId);
        }
    }
}
