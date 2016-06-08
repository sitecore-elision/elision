using System;
using Elision.Seo.Caching;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Sites;

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

        protected virtual void WriteResponse(string errorPageUrl, int statusCode, Action fallbackAction)
        {
            var context = System.Web.HttpContext.Current;
            try
            {
                var domain = context.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
                var fullUrl = string.Concat(domain, errorPageUrl);

                if (!ManagedErrorPageExists(fullUrl))
                {
                    fallbackAction();
                    return;
                }

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

        protected virtual bool ManagedErrorPageExists(string fullUrl)
        {
            var url = new Uri(fullUrl, UriKind.Absolute);

            var siteContext = SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery, url.Port);

            var homePath = siteContext.StartPath;
            if (!homePath.EndsWith("/"))
                homePath += "/";

            var itemPath = MainUtil.DecodeName(url.AbsolutePath);
            if (itemPath.StartsWith(siteContext.VirtualFolder))
                itemPath = itemPath.Remove(0, siteContext.VirtualFolder.Length);

            var fullPath = homePath + itemPath;
            return siteContext.Database.GetItem(fullPath) != null;
        }
    }
}
