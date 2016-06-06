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
    }
}
