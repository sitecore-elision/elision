using System.Web;
using Elision.Themes;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Elision.Areas.Elision.Models
{
    public interface ISiteResourceModelBuilder
    {
        SiteResourceViewModel Build(Item renderingContextItem, ID resourceLocationId, ID deviceId, string siteScriptFieldName, string pageScriptFieldName);
    }

    public class SiteResourceModelBuilder : ISiteResourceModelBuilder
    {
        private readonly IThemeRetriever _themeRetriever;

        public SiteResourceModelBuilder(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public SiteResourceViewModel Build(Item renderingContextItem, ID resourceLocationId, ID deviceId,
            string siteScriptFieldName, string pageScriptFieldName)
        {
            var model = new SiteResourceViewModel();

            if (!string.IsNullOrWhiteSpace(siteScriptFieldName))
            {
                model.SiteResources =
                    new HtmlString(string.Join("\r\n",
                        (renderingContextItem ?? Sitecore.Context.Item).GetInheritedFieldValue(siteScriptFieldName))
                        .Or(""));
            }

            if (renderingContextItem != null)
            {
                if (!string.IsNullOrWhiteSpace(pageScriptFieldName))
                {
                    model.PageResources = new HtmlString(renderingContextItem[pageScriptFieldName] ?? "");
                }

                var theme = _themeRetriever.GetThemeFromContextItem(renderingContextItem);
                model.ThemeResources = new HtmlString(theme == null
                    ? ""
                    : _themeRetriever.GetThemeResources(theme, deviceId, resourceLocationId)
                    );
            }

            return model;
        }
    }
}