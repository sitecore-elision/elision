using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Elision.Foundation.Search.ComputedFields
{
    public class BaseTemplates : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null) return null;

            var baseTemplates = new List<ID> {indexItem.TemplateID};

            baseTemplates.AddRange(GetBaseTemplateIds(indexItem.Template));

            return baseTemplates.Distinct().ToArray();
        }

        protected virtual IEnumerable<ID> GetBaseTemplateIds(TemplateItem template)
        {
            foreach (var baseTemplate in template.BaseTemplates)
            {
                yield return baseTemplate.ID;
                foreach (var subTemplate in GetBaseTemplateIds(baseTemplate))
                {
                    yield return subTemplate;
                }
            }
        } 

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}
