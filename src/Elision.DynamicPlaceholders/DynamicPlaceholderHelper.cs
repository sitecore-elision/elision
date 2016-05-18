using System.Web;
using Elision.WebEditState;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;

namespace Elision.DynamicPlaceholders
{
    public static class DynamicPlaceholderHelper
    {
        public static HtmlString DynamicPlaceholder(this Sitecore.Mvc.Helpers.SitecoreHelper helper, string key, object parameters = null)
        {
            var disableWebEdit = parameters != null && TypeHelper.GetPropertyValue<bool>("disablewebedit", parameters);
            var currentRenderingId = RenderingContext.Current.Rendering.UniqueId;

            WebEditDisabler disabler = null;
            try
            {
                if (disableWebEdit)
                    disabler = new WebEditDisabler();

                return helper.Placeholder($"{key}_{currentRenderingId}");
            }
            finally
            {
                disabler?.Dispose();
            }
        }
    }
}
