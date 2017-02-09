using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Elision.Foundation.Mvc.Pipelines.GetControllerRenderingValueParameters;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Presentation;

namespace Elision.Foundation.Mvc.ValueProviders
{
    public class PipelineValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (RenderingContext.CurrentOrNull?.Rendering?.Parameters == null)
                return null;

            var args = new GetControllerRenderingValueParametersArgs(HttpContext.Current, RenderingContext.Current);

            var parameters = PipelineService.Get().RunPipeline("elision.getControllerRenderingValueParameters",
                                                               args,
                                                               x => x.Parameters);

            return new PipelineValueProvider(parameters ?? new Dictionary<string, object>(), CultureInfo.CurrentCulture);
        }
    }
}
