using System.Web.Mvc;
using Sitecore.Pipelines;

namespace Elision.Ioc.Mvc
{
    public class InitializeControllerFactory : Sitecore.Mvc.Pipelines.Loader.InitializeControllerFactory
    {
        protected override void SetControllerFactory(PipelineArgs args)
        {
            ControllerBuilder.Current
                             .SetControllerFactory(
                                 new ElisionControllerFactory(ControllerBuilder.Current.GetControllerFactory()));
        }
    }
}
