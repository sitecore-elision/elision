using System.Web.Mvc;
using Elision.Areas.Elision.Models;
using Elision.Themes;
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

        public ActionResult Head(Item renderingContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(renderingContextItem,
                Options.ResourceLocations.Head,
                new ID(pageContext.Device.Id),
                Templates._SiteScripts.FieldNames.SiteHeadScript,
                Templates._PageScripts.FieldNames.PageHeadScript);

            return View(model);
        }

        public ActionResult BodyTop(Item renderingContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(renderingContextItem,
                Options.ResourceLocations.BodyTop,
                new ID(pageContext.Device.Id),
                Templates._SiteScripts.FieldNames.SiteBodyTopScript,
                Templates._PageScripts.FieldNames.PageBodyTopScript);

            return View(model);
        }

        public ActionResult BodyBottom(Item renderingContextItem, PageContext pageContext)
        {
            var model = _modelBuilder.Build(renderingContextItem,
                                            Options.ResourceLocations.BodyBottom,
                                            new ID(pageContext.Device.Id),
                                            Templates._SiteScripts.FieldNames.SiteFootScript,
                                            Templates._PageScripts.FieldNames.PageFootScript);

            return View(model);
        }
    }
}