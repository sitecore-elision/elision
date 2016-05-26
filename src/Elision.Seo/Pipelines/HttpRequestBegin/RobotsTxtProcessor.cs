using System;
using System.Web;
using Sitecore.Pipelines.HttpRequest;

namespace Elision.Seo.Pipelines.HttpRequestBegin
{
    public class RobotsTxtProcessor : HttpRequestProcessor, IHttpHandler
    {
        public override void Process(HttpRequestArgs args)
        {
            var context = HttpContext.Current;
            if (context == null)
                return;

            ProcessRequest(context);
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();

            //var requestUrl = context.Request.RawUrl;
            //if (string.IsNullOrWhiteSpace(requestUrl) || !requestUrl.ToLower().EndsWith("robots.txt"))
            //    return;

            //var robotsTxtContent = @"User-agent: *" + Environment.NewLine
            //                       + "Disallow: /sitecore" + Environment.NewLine
            //                       + "Sitemap: /sitemap.xml";

            //if (Sitecore.Context.Site != null && Sitecore.Context.Database != null)
            //{
            //    var homeNode =
            //        Sitecore.Context.Database.GetItem(Sitecore.Context.Site.RootPath + Sitecore.Context.Site.StartItem);
            //    if (homeNode != null)
            //    {
            //        robotsTxtContent = homeNode.Fields.GetValue(Templates._RobotsTxtGeneration.FieldIds.RobotsTxt)
            //                                   .Or(robotsTxtContent);
            //    }
            //}

            //context.Response.ContentType = "text/plain";
            //context.Response.Write(robotsTxtContent);
            //context.Response.End();
        }

        public bool IsReusable => true;
    }
}
