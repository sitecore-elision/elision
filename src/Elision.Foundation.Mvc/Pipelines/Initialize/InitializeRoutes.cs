using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Elision.Foundation.Mvc.Pipelines.Initialize
{
    public class InitializeRoutes : Sitecore.Mvc.Pipelines.Loader.InitializeRoutes
    {
        public override void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapMvcAttributeRoutes();
            base.Process(args);
        }
    }
}
