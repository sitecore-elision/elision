using Elision.Seo.Pipelines.GetCanonicalUrl;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.Seo
{
    public static class CanonicalUrlExtensions
    {
        public static string GetCanonicalUrl(this Item pageItem, string rawRequestUrl = null)
        {
            var getCanonicalUrlArgs = new GetCanonicalUrlArgs(pageItem, rawRequestUrl);
            CorePipeline.Run("elision.getCanonicalUrl", getCanonicalUrlArgs);
            return getCanonicalUrlArgs.CanonicalUrl;
        }
    }
}