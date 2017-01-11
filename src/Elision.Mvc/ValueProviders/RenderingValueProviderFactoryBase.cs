using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace Elision.Foundation.Mvc.ValueProviders
{
    public abstract class RenderingValueProviderFactoryBase : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var httpContext = controllerContext.HttpContext;
            var renderingContext = RenderingContext.CurrentOrNull;
            if (httpContext == null || renderingContext == null)
                return null;

            return GetValueProvider(httpContext, renderingContext);
        }

        protected abstract IValueProvider GetValueProvider(HttpContextBase httpContext, RenderingContext renderingContext);
    }
}
