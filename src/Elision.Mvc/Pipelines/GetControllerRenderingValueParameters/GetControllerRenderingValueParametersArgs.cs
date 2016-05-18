using System.Collections.Generic;
using System.Web;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Presentation;

namespace Elision.Mvc.Pipelines.GetControllerRenderingValueParameters
{
    public class GetControllerRenderingValueParametersArgs : MvcPipelineArgs
    {
        public HttpContextBase HttpContext { get; set; }
        public RenderingContext RenderingContext { get; set; }

        public readonly Dictionary<string, object> Parameters = new Dictionary<string, object>();

        public GetControllerRenderingValueParametersArgs(HttpContextBase httpContext, RenderingContext renderingContext)
        {
            HttpContext = httpContext;
            RenderingContext = renderingContext;
        }
    }
}