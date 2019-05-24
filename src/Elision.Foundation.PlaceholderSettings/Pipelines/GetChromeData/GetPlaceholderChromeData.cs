using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
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
            //Assert.ArgumentNotNull(args, nameof(args));
            //Assert.IsNotNull(args.ChromeData, "Chrome Data");
            //if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            //    return;
            //var placeholderKey = args.CustomData["placeHolderKey"] as string;
            //Assert.IsNotNull(placeholderKey, "CustomData[\"{0}\"]", "placeHolderKey");
            //var lastPart = StringUtil.GetLastPart(placeholderKey, '/', placeholderKey);
            //args.ChromeData.DisplayName = lastPart;
            //AddButtonsToChromeData(GetButtons("/sitecore/content/Applications/WebEdit/Default Placeholder Buttons"), args);
            //var placeholderItem = (Item)null;
            //var hasPlaceholderSettings = false;
            //if (args.Item != null)
            //{
            //    var layout = ChromeContext.GetLayout(args.Item);
            //    var placeholderRenderingsArgs = new GetPlaceholderRenderingsArgs(placeholderKey, layout, args.Item.Database)
            //    {
            //        OmitNonEditableRenderings = true
            //    };

            //    //add full dynamic placeholder key to reference in applicable rules
            //    placeholderRenderingsArgs.CustomData.Add("fullPlaceholderKey", placeholderKey);

            //    CorePipeline.Run("getPlaceholderRenderings", placeholderRenderingsArgs);
            //    hasPlaceholderSettings = placeholderRenderingsArgs.HasPlaceholderSettings;
            //    var getRenderingList = placeholderRenderingsArgs.PlaceholderRenderings != null
            //        ? placeholderRenderingsArgs.PlaceholderRenderings.Select(i => i.ID.ToShortID().ToString()).ToList()
            //        : new List<string>();

            //    if (args.ChromeData.Custom.ContainsKey("allowedRenderings"))
            //    {
            //        var valueList = args.ChromeData.Custom.Where(x => x.Key.Equals("allowedRenderings")).Select(x => x.Value).ToList();
            //        valueList.Add(getRenderingList);
            //        args.ChromeData.Custom["allowedRenderings"] = valueList;

            //    }
            //    else
            //    {
            //        args.ChromeData.Custom.Add("allowedRenderings", getRenderingList);
            //    }
            //    placeholderItem = Client.Page.GetPlaceholderItem(placeholderRenderingsArgs.PlaceholderKey, args.Item.Database, layout);
            //    args.ChromeData.DisplayName = placeholderItem == null ? StringUtil.GetLastPart(placeholderRenderingsArgs.PlaceholderKey, '/', placeholderRenderingsArgs.PlaceholderKey) : HttpUtility.HtmlEncode(placeholderItem.DisplayName);
            //    if (placeholderItem != null && !string.IsNullOrEmpty(placeholderItem.Appearance.ShortDescription))
            //        args.ChromeData.ExpandedDisplayName = HttpUtility.HtmlEncode(placeholderItem.Appearance.ShortDescription);
            //}
            //else
            //{
            //    args.ChromeData.Custom.Add("allowedRenderings", new List<string>());
            //}


            //var isEditable = (placeholderItem == null || placeholderItem["Editable"] == "1") && Settings.WebEdit.PlaceholdersEditableWithoutSettings | hasPlaceholderSettings;

            //if (args.ChromeData.Custom.ContainsKey("editable"))
            //{
            //    args.ChromeData.Custom["editable"] = isEditable.ToString().ToLowerInvariant();
            //}
            //else
            //{
            //    args.ChromeData.Custom.Add("editable", isEditable.ToString().ToLowerInvariant());
            //}

            Assert.ArgumentNotNull((object)args, nameof(args));
            Assert.IsNotNull((object)args.ChromeData, "Chrome Data");
            if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
                return;
            string str = args.CustomData["placeHolderKey"] as string;
            Assert.IsNotNull((object)str, "CustomData[\"{0}\"]", "placeHolderKey");
            string lastPart = StringUtil.GetLastPart(str, '/', str);
            args.ChromeData.DisplayName = lastPart;
            this.AddButtonsToChromeData((IEnumerable<WebEditButton>)base.GetButtons("/sitecore/content/Applications/WebEdit/Default Placeholder Buttons"), args);
            Item obj = (Item)null;
            bool flag1 = false;
            if (args.Item != null)
            {
                string layout = ChromeContext.GetLayout(args.Item);
                GetPlaceholderRenderingsArgs placeholderRenderingsArgs = new GetPlaceholderRenderingsArgs(str, layout, args.Item.Database)
                {
                    OmitNonEditableRenderings = true
                };
                placeholderRenderingsArgs.CustomData.Add("fullPlaceholderKey", str);
                CorePipeline.Run("getPlaceholderRenderings", (PipelineArgs)placeholderRenderingsArgs);
                flag1 = placeholderRenderingsArgs.HasPlaceholderSettings;
                List<string> stringList = placeholderRenderingsArgs.PlaceholderRenderings != null ? placeholderRenderingsArgs.PlaceholderRenderings.Select<Item, string>((Func<Item, string>)(i => i.ID.ToShortID().ToString())).ToList<string>() : new List<string>();
                if (args.ChromeData.Custom.ContainsKey("allowedRenderings"))
                {
                    var valueList = args.ChromeData.Custom.Where(x => x.Key.Equals("allowedRenderings")).Select(x => x.Value).ToList();
                    valueList.Add(stringList);
                    args.ChromeData.Custom["allowedRenderings"] = valueList;

                }
                else
                {
                    args.ChromeData.Custom.Add("allowedRenderings", stringList);
                }                
                obj = Sitecore.Client.Page.GetPlaceholderItem(placeholderRenderingsArgs.PlaceholderKey, args.Item.Database, layout);
                args.ChromeData.DisplayName = obj == null ? StringUtil.GetLastPart(placeholderRenderingsArgs.PlaceholderKey, '/', placeholderRenderingsArgs.PlaceholderKey) : HttpUtility.HtmlEncode(obj.DisplayName);
                if (obj != null && !string.IsNullOrEmpty(obj.Appearance.ShortDescription))
                    args.ChromeData.ExpandedDisplayName = HttpUtility.HtmlEncode(obj.Appearance.ShortDescription);
            }
            else
            {
                if (args.ChromeData.Custom.ContainsKey("allowedRenderings"))
                {
                    var valueList = args.ChromeData.Custom.Where(x => x.Key.Equals("allowedRenderings")).Select(x => x.Value).ToList();
                    valueList.Add((object)new List<string>());
                    args.ChromeData.Custom["allowedRenderings"] = valueList;

                }
                else
                {
                    args.ChromeData.Custom.Add("allowedRenderings", (object)new List<string>());
                }
            }

            bool flag2 = (obj == null || obj["Editable"] == "1") && Settings.WebEdit.PlaceholdersEditableWithoutSettings | flag1;
            if (args.ChromeData.Custom.ContainsKey("editable"))
            {
                args.ChromeData.Custom["editable"] = (object)flag2.ToString().ToLowerInvariant();
            }
            else
            {
                args.ChromeData.Custom.Add("editable", (object)flag2.ToString().ToLowerInvariant());
            }
        }
    }
}
