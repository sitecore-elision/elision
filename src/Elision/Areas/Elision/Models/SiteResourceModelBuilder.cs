using Elision.SiteResources.Pipelines.GetSiteResources;
using Elision.Themes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.Areas.Elision.Models
{
    public interface ISiteResourceModelBuilder
    {
        SiteResourceViewModel Build(Item renderingContextItem, ID resourceLocationId, ID deviceId);
    }

    public class SiteResourceModelBuilder : ISiteResourceModelBuilder
    {
        private readonly IThemeRetriever _themeRetriever;

        public SiteResourceModelBuilder(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public virtual SiteResourceViewModel Build(Item renderingContextItem, ID resourceLocationId, ID deviceId)
        {
            var model = new SiteResourceViewModel();

            var args = new GetSiteResourcesArgs(renderingContextItem, resourceLocationId, deviceId);

            CorePipeline.Run("getSiteResources", args);

            model.Resources = args.Results;

            return model;
        }
    }
}