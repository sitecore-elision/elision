using System.Collections.Generic;
using System.Web;
using Elision.Foundation.Kernel;
using Elision.Seo;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Elision.Foundation.Areas.Elision.Models
{
    public interface IPageMetadataModelBuilder
    {
        PageMetadataModel Build(Item item);
    }

    public class PageMetadataModelBuilder : IPageMetadataModelBuilder
    {
        public virtual PageMetadataModel Build(Item item)
        {
            var urlOptions = (UrlOptions)UrlOptions.DefaultOptions.Clone();
            urlOptions.AlwaysIncludeServerUrl = true;
            urlOptions.LanguageEmbedding = LanguageEmbedding.Never;

            var model = new PageMetadataModel
            {
                BrowserTitle = GetBrowserTitle(item),
                OgSiteName = item.Fields.GetValue(Templates._OpenGraphMeta.FieldIds.OgSiteName),
                OgType = item.Fields.GetValue(Templates._OpenGraphMeta.FieldIds.OgType),
                OgTitle = item.Fields.GetValue(Templates._OpenGraphMeta.FieldIds.OgTitle),
                OgDescription = item.Fields.GetValue(Templates._OpenGraphMeta.FieldIds.OgDescription),
                MetaKeywords = item.Fields.GetValue(Templates._PageMetaFields.FieldIds.MetaKeywords),
                MetaDescription = item.Fields.GetValue(Templates._PageMetaFields.FieldIds.MetaDescription),
                Language = item.Language.CultureInfo.Name,
                LastModified = item.Statistics.Updated,
                Url = LinkManager.GetItemUrl(item, urlOptions),
                CanonicalUrl = GetCanonicalUrl(item),
                RobotsMeta = GetRobotsMeta(item),
                OgImage = GetOgImage(item)
            };

            return model;
        }

        protected virtual string GetCanonicalUrl(Item item)
        {
            return item.GetCanonicalUrl(HttpContext.Current?.Request.RawUrl);
        }

        protected virtual string GetBrowserTitle(Item item)
        {
            var title = item.Fields.GetValue(Templates._PageMetaFields.FieldIds.BrowserTitle).Or(item.DisplayName).Or(item.Name);
            return title + item.GetInheritedFieldValue(Templates._SiteMetaFields.FieldNames.BrowserTitleSuffix, true);
        }

        protected virtual OgImageModel GetOgImage(Item item)
        {
            var ogImageField = (ImageField) item.Fields[Templates._OpenGraphMeta.FieldIds.OgImage];
            if (ogImageField == null) 
                return null;

            var mediaItem = (MediaItem) ogImageField.MediaItem;
            if (mediaItem == null)
                return null;

            var mediaUrlOptions = MediaUrlOptions.Empty;
            mediaUrlOptions.AbsolutePath = true;
            mediaUrlOptions.AlwaysIncludeServerUrl = true;

            int size;
            return new OgImageModel
                {
                    Url = MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions),
                    MimeType = mediaItem.MimeType,
                    Width = int.TryParse(ogImageField.Width, out size) ? size : 0,
                    Height = int.TryParse(ogImageField.Height, out size) ? size : 0
                };
        }

        protected virtual string GetRobotsMeta(Item pageItem)
        {
            var flags = new List<string>();

            if (pageItem.Fields.GetValue(Templates._PageMetaFields.FieldIds.BlockSearchEngineIndexing) == "1")
                flags.Add("noindex");
            if (pageItem.Fields.GetValue(Templates._PageMetaFields.FieldIds.BlockSearchEngineLinkFollowing) == "1")
                flags.Add("nofollow");

            return string.Join(",", flags);
        }
    }
}