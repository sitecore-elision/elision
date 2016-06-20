using Elision.SiteResources;
using Elision.SiteResources.Pipelines.GetSiteResources;

namespace Elision.Themes.GetSiteResources
{
    public class GetThemeResources : IGetSiteResourcesPipelineHandler
    {
        private readonly IThemeRetriever _themeRetriever;

        public GetThemeResources(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public void Process(GetSiteResourcesArgs args)
        {
            if (args.ContextItem == null)
                return;

            var theme = _themeRetriever.GetThemeFromContextItem(args.ContextItem);
            if (theme == null)
                return;

            var themeScripts = _themeRetriever.GetThemeResources(theme, args.DeviceId, args.ResourceLocationId);
            if (string.IsNullOrWhiteSpace(themeScripts))
                return;

            args.Results.Add(new SiteResource("ThemeResources", themeScripts));
        }
    }
}