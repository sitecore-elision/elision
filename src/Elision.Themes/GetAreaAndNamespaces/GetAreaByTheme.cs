using Elision.Foundation.Kernel;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Elision.Foundation.Themes.GetAreaAndNamespaces
{
    public class GetAreaByTheme : IAreaResolveStrategy
    {
        private readonly IThemeRetriever _themeRetriever;

        public GetAreaByTheme(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public string Resolve(RenderRenderingArgs args)
        {
            var theme = _themeRetriever.GetThemeFromContextItem(args.PageContext.Item);
            var areaName = theme?[Templates.Theme.FieldNames.MvcAreaName];
            return string.IsNullOrWhiteSpace(areaName)
                ? null
                : areaName;
        }
    }
}