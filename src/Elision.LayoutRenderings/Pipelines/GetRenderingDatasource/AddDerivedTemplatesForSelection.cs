using System.Linq;
using Elision.Diagnostics;
using Sitecore.Data.Managers;
using Sitecore.Pipelines.GetRenderingDatasource;

namespace Elision.LayoutRenderings.Pipelines.GetRenderingDatasource
{
    public class AddDerivedTemplatesForSelection
    {
        public void Process(GetRenderingDatasourceArgs args)
        {
            var db = args.Prototype == null ? args.ContentDatabase : args.Prototype.Database;

            using (new TraceOperation($"Add derived templates for selection in {(args.RenderingItem == null ? "rendering" : args.RenderingItem.Name)} rendering datasource dialog."))
            {
                var index = 0;
                while (index < args.TemplatesForSelection.Count)
                {
                    var template = args.TemplatesForSelection[index];

                    var allDerived = db.SelectItems("/sitecore/templates//*[contains(@#__Base template#, '" + template.ID + "')]");

                    foreach (var newTemplate in allDerived.Select(TemplateManager.GetTemplate))
                    {
                        if (!args.TemplatesForSelection.Contains(newTemplate))
                            args.TemplatesForSelection.Add(newTemplate);
                    }

                    index++;
                }
            }
        }
    }
}
