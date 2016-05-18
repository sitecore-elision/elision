using System;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Xml.Xsl;

namespace Elision
{
    public static class ItemLinkExtensions
    {
        public static string Url(this Item item, UrlOptions options = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return options != null ? LinkManager.GetItemUrl(item, options) : LinkManager.GetItemUrl(item);
        }

        public static string ImageUrl(this Item item, string imageFieldName, int? width = null, int? height = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldName];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(width, height);
        }

        public static string ImageUrl(this Item item, ID imageFieldId, int? width = null, int? height = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldId];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(width, height);
        }

        public static string ImageUrl(this Item item, string imageFieldName, MediaUrlOptions options)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldName];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
        }

        public static string ImageUrl(this Item item, ID imageFieldId, MediaUrlOptions options)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldId];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
        }

        public static string MediaUrl(this Item item, ID mediaFieldId, MediaUrlOptions options = null)
        {
            var targetItem = item.TargetItem(mediaFieldId);
            if (targetItem == null)
                return string.Empty;

            return (options == null
                ? MediaManager.GetMediaUrl(targetItem)
                : MediaManager.GetMediaUrl(targetItem, options))
                   ?? string.Empty;
        }

        public static string MediaUrl(this Item item, string mediaFieldName, MediaUrlOptions options = null)
        {
            var targetItem = item.TargetItem(mediaFieldName);
            if (targetItem == null)
                return string.Empty;

            return (options == null
                ? MediaManager.GetMediaUrl(targetItem)
                : MediaManager.GetMediaUrl(targetItem, options))
                   ?? string.Empty;
        }

        public static Item TargetItem(this Item item, ID linkFieldId)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var linkField = (LinkField)item.Fields[linkFieldId];
            return linkField?.TargetItem;
        }

        public static Item TargetItem(this Item item, string linkFieldName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var linkField = (LinkField)item.Fields[linkFieldName];
            return linkField?.TargetItem;
        }

        public static string LinkFieldUrl(this Item item, ID linkFieldId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (ID.IsNullOrEmpty(linkFieldId))
                throw new ArgumentNullException(nameof(linkFieldId));

            var field = item.Fields[linkFieldId];
            if (field == null)
                return string.Empty;

            var linkUrl = new LinkUrl();
            return linkUrl.GetUrl(item, linkFieldId.ToString());
        }

        public static string LinkFieldTarget(this Item item, ID fieldId)
        {
            XmlField field = item.Fields[fieldId];
            return field?.GetAttribute("target");
        }
    }
}
