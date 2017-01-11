using System.Web.Mvc;
using Elision.Foundation.Areas.Elision.Models;
using Elision.Foundation.Kernel;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;

namespace Elision.Foundation.Areas.Elision.Controllers
{
    public class TwitterController : SitecoreController
    {
        public ActionResult CardMeta(Item pageContextItem)
        {
            var model = new TwitterCardMetaViewModel
                {
                    CardType = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardType],
                    Site = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardSite],
                    SiteId = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardSiteId],
                    Title = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardTitle],
                    Description = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardDescription],
                    ContentCreator = pageContextItem[Templates._TwitterCardsMeta.FieldNames.TwitterCardContentCreator],
                    ImageUrl = pageContextItem.MediaUrl(Templates._TwitterCardsMeta.FieldNames.TwitterCardImage)
                };

            return View(model);
        }
    }
}