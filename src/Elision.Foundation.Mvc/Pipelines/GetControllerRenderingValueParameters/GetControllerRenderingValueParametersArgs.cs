using System.Collections.Generic;
using System.Web;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Presentation;

namespace Elision.Foundation.Mvc.Pipelines.GetControllerRenderingValueParameters
{
    public class GetControllerRenderingValueParametersArgs : MvcPipelineArgs
    {
        public HttpContext HttpContext { get; set; }
        public RenderingContext RenderingContext { get; set; }

        public readonly Dictionary<string, object> Parameters = new Dictionary<string, object>();

        public GetControllerRenderingValueParametersArgs(HttpContext httpContext, RenderingContext renderingContext)
        {
            HttpContext = httpContext;
            RenderingContext = renderingContext;
        }
    }
}