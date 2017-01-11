using Elision.Foundation.Kernel;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Elision.Foundation.Mvc.Pipelines.GetAreaAndNamespaces
{
    public class GetAreaByRenderingFolder : IAreaResolveStrategy
    {
        public string Resolve(RenderRenderingArgs args)
        {
            return FindAreaByFolder(args);
        }

        public virtual string FindAreaByFolder(RenderRenderingArgs args)
        {
            var current = args.Rendering?.RenderingItem?.InnerItem;
            if (current == null)
                return null;

            var areaName = current.GetInheritedFieldValue(Templates.MvcAreaName.FieldNames.AreaName, true);

            return string.IsNullOrWhiteSpace(areaName)
                       ? null
                       : areaName;
        }
    }
}