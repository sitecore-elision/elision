using System;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Sites;
using Sitecore.Web;

namespace Elision.Foundation.Kernel
{
    public  static class SiteExtensions
    {
        public static Item GetStartItem(this SiteContext site)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            return site.Database.GetItem(site.StartPath);
        }

        public static Item GetStartItem(this SiteInfo site)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            var database = Factory.GetDatabase(site.Database);
            return database.GetItem(site.RootPath + site.StartItem);
        }
    }
}
