using System;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;

namespace Elision
{
    public static class MediaExtensions
    {
        public static string ImageUrl(this ImageField imageField, int? width = null, int? height = null)
        {
            if (imageField?.MediaItem == null)
                throw new ArgumentNullException(nameof(imageField));

            var options = MediaUrlOptions.Empty;
            int parsedInt;

            if (width.HasValue)
                options.Width = width.Value;
            else if (int.TryParse(imageField.Width, out parsedInt))
                options.Width = parsedInt;

            if (height.HasValue)
                options.Height = height.Value;
            else if (int.TryParse(imageField.Height, out parsedInt))
                options.Height = parsedInt;

            return imageField.ImageUrl(options);
        }

        public static string ImageUrl(this ImageField imageField, MediaUrlOptions options)
        {
            if (imageField?.MediaItem == null)
                throw new ArgumentNullException(nameof(imageField));

            return options == null ? imageField.ImageUrl() : HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(imageField.MediaItem, options));
        }

        public static bool IsChecked(this Field checkboxField)
        {
            if (checkboxField == null)
                throw new ArgumentNullException(nameof(checkboxField));

            return MainUtil.GetBool(checkboxField.Value, false);
        }
    }
}
