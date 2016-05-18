using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Elision.Mvc.Pipelines.GetControllerRenderingValueParameters;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Presentation;

namespace Elision.Mvc.ValueProviders
{
    public class PipelineValueProviderFactory : RenderingValueProviderFactoryBase
    {
        protected override IValueProvider GetValueProvider(HttpContextBase httpContext, RenderingContext renderingContext)
        {
            if (renderingContext.Rendering.Parameters == null)
                return null;

            var args = new GetControllerRenderingValueParametersArgs(httpContext, renderingContext);

            var parameters = PipelineService.Get().RunPipeline("elision.getControllerRenderingValueParameters",
                                                               args,
                                                               x => x.Parameters);

            return new PipelineValueProvider(parameters ?? new Dictionary<string, object>(), CultureInfo.CurrentCulture);
        }
    }
}
