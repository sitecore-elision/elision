using System;
using Elision.Seo.Caching;
using Sitecore.Configuration;

namespace Elision.Seo.Pipelines.HttpRequestBegin
{
    public class MissingItemExecuteRequest : Sitecore.Pipelines.HttpRequest.ExecuteRequest
    {
        protected override void RedirectOnItemNotFound(string url)
        {
            WriteResponse(
                Settings.GetSetting("ItemNotFoundUrlManaged", url),
                Settings.GetIntSetting("ItemNotFoundUrlErrorCode", 404),
                () => base.RedirectOnItemNotFound(url));
        }

        protected override void RedirectOnLayoutNotFound(string url)
        {
            WriteResponse(
                Settings.GetSetting("LayoutNotFoundUrlManaged", url),
                Settings.GetIntSetting("LayoutNotFoundUrlErrorCode", 500),
                () => base.RedirectOnLayoutNotFound(url));
        }

        protected override void RedirectOnNoAccess(string url)
        {
            WriteResponse(
                Settings.GetSetting("NoAccessUrlManaged", url),
                Settings.GetIntSetting("NoAccessUrlErrorCode", 403),
                () => base.RedirectOnNoAccess(url));
        }

        protected override void RedirectOnSiteAccessDenied(string url)
        {
            WriteResponse(
                Settings.GetSetting("NoAccessUrlManaged", url),
                Settings.GetIntSetting("NoAccessUrlErrorCode", 403),
                () => base.RedirectOnSiteAccessDenied(url));
        }

        private static void WriteResponse(string errorPageUrl, int statusCode, Action fallbackAction)
        {
            var context = System.Web.HttpContext.Current;
            try
            {
                var domain = context.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
                var fullUrl = string.Concat(domain, errorPageUrl);

                var content = ErrorPagesResponseCache.Current.Get(fullUrl);
                if (string.IsNullOrWhiteSpace(content))
                {
                    content = Sitecore.Web.WebUtil.ExecuteWebPage(fullUrl);
                    ErrorPagesResponseCache.Current.Set(fullUrl, content);
                }
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = statusCode;
                context.Response.Write(content);
                context.Response.End();
            }
            catch (Exception)
            {
                fallbackAction();
            }
        }
    }
}
