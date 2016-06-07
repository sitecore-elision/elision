using System.Web;
using Sitecore;
using Sitecore.Mvc.Helpers;

namespace Elision.Mvc
{
    public static class SitecoreHelperExtensions
    {
        public static bool IsInEditMode(this SitecoreHelper helper)
        {
            return Context.PageMode.IsExperienceEditorEditing;
        }

        public static HtmlString TranslateText(this SitecoreHelper helper, string key)
        {
            return new HtmlString(Sitecore.Globalization.Translate.Text(key));
        }

        public static HtmlString TranslateText(this SitecoreHelper helper, string key, params object[] parameters)
        {
            return new HtmlString(Sitecore.Globalization.Translate.Text(key, parameters));
        }
    }
}
