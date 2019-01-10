using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using Sitecore.Web.UI.PageModes;

namespace Elision.Foundation.PlaceholderSettings.Pipelines.GetChromeData
{
    public class GetPlaceholderChromeData : GetChromeDataProcessor
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
                return;
            var placeholderKey = args.CustomData["placeHolderKey"] as string;
            Assert.IsNotNull(placeholderKey, "CustomData[\"{0}\"]", "placeHolderKey");
            var lastPart = StringUtil.GetLastPart(placeholderKey, '/', placeholderKey);
            args.ChromeData.DisplayName = lastPart;
            AddButtonsToChromeData(GetButtons("/sitecore/content/Applications/WebEdit/Default Placeholder Buttons"), args);
            var placeholderItem = (Item)null;
            var hasPlaceholderSettings = false;
            if (args.Item != null)
            {
                var layout = ChromeContext.GetLayout(args.Item);
                var placeholderRenderingsArgs = new GetPlaceholderRenderingsArgs(placeholderKey, layout, args.Item.Database)
                {
                    OmitNonEditableRenderings = true
                };

                //add full dynamic placeholder key to reference in applicable rules
                placeholderRenderingsArgs.CustomData.Add("fullPlaceholderKey", placeholderKey);

                CorePipeline.Run("getPlaceholderRenderings", placeholderRenderingsArgs);
                hasPlaceholderSettings = placeholderRenderingsArgs.HasPlaceholderSettings;
                var getRenderingList = placeholderRenderingsArgs.PlaceholderRenderings != null
                    ? placeholderRenderingsArgs.PlaceholderRenderings.Select(i => i.ID.ToShortID().ToString()).ToList()
                    : new List<string>();
                args.ChromeData.Custom.Add("allowedRenderings", getRenderingList);
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderRenderingsArgs.PlaceholderKey, args.Item.Database, layout);
                args.ChromeData.DisplayName = placeholderItem == null ? StringUtil.GetLastPart(placeholderRenderingsArgs.PlaceholderKey, '/', placeholderRenderingsArgs.PlaceholderKey) : HttpUtility.HtmlEncode(placeholderItem.DisplayName);
                if (placeholderItem != null && !string.IsNullOrEmpty(placeholderItem.Appearance.ShortDescription))
                    args.ChromeData.ExpandedDisplayName = HttpUtility.HtmlEncode(placeholderItem.Appearance.ShortDescription);
            }
            else
            {
                args.ChromeData.Custom.Add("allowedRenderings", new List<string>());
            }
                
            var isEditable = (placeholderItem == null || placeholderItem["Editable"] == "1") && Settings.WebEdit.PlaceholdersEditableWithoutSettings | hasPlaceholderSettings;
            args.ChromeData.Custom.Add("editable", isEditable.ToString().ToLowerInvariant());                        
        }
    }
}
