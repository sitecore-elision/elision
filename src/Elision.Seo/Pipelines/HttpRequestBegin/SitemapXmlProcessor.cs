using System;
using System.Web;
using Elision.Seo.Pipelines.GenerateSitemapXml;
using Sitecore.Pipelines;

namespace Elision.Seo.Pipelines.HttpRequestBegin
{
    public class SitemapXmlProcessor : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();

            //var args = new GenerateSitemapArgs()
            //    {
            //        RequestUrl = context.Request.RawUrl
            //    };
            //CorePipeline.Run("getSitemapXml", args);
            
            //if (string.IsNullOrWhiteSpace(args.Content))
            //    return;

            //context.Response.ContentType = "application/xml";
            //context.Response.Write(args.Content);
            //context.Response.End();
        }

        public bool IsReusable => true;
    }
}
