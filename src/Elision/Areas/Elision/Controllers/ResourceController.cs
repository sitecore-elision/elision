using System.Web.Mvc;
using Elision.Areas.Elision.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Elision.Areas.Elision.Controllers
{
    public class ResourceController : SitecoreController
    {
        private readonly ISiteResourceModelBuilder _modelBuilder;

        public ResourceController(ISiteResourceModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public ActionResult Head(Item pageContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(pageContextItem, OptionLists.ResourceLocations.Head, new ID(pageContext.Device.Id));
            return View(model);
        }

        public ActionResult BodyTop(Item pageContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(pageContextItem, OptionLists.ResourceLocations.Body_Top, new ID(pageContext.Device.Id));
            return View(model);
        }

        public ActionResult BodyBottom(Item pageContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(pageContextItem, OptionLists.ResourceLocations.Body_Bottom, new ID(pageContext.Device.Id));
            return View(model);
        }
    }
}