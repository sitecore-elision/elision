using Elision.Seo.Areas.Elision.Models;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Elision.Seo.Areas.Elision.Controllers
{
    public class MetadataController : SitecoreController
    {
        private readonly IPageMetadataModelBuilder _modelBuilder;

        public MetadataController(IPageMetadataModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public System.Web.Mvc.ActionResult PageMetadata()
        {
            var model = _modelBuilder.Build(RenderingContext.Current.ContextItem);
            return View(model);
        }
    }
}
