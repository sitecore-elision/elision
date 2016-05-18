using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Layouts;

namespace Elision.DynamicPlaceholders
{
    public class FixOrphanedRenderings
    {
        public void OnItemSaving(object sender, EventArgs args)
        {
            if (Context.Device == null) return;

            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null) return;

            var regex = new Regex(GetDynamicKeyAllowedRenderings.DynamicKeyPattern);

            var layoutDef = item.GetLayoutDefinition();

            foreach (var deviceDef in layoutDef.Devices.Cast<DeviceDefinition>())
            {
                if (deviceDef == null || deviceDef.Renderings == null) return;

                var renderingsToRemove = new List<RenderingDefinition>();
                var phKeysToUpdate = new Dictionary<string, string>();

                var renderingReferences = deviceDef.Renderings.Cast<RenderingDefinition>()
                                                   .Where(x => x != null && !string.IsNullOrWhiteSpace(x.Placeholder))
                                                   .ToArray();
                foreach (var renderingReference in renderingReferences)
                {
                    var match = regex.Match(renderingReference.Placeholder);
                    if (!match.Success) continue;

                    var parentRenderingId = ID.Parse("{" + match.Groups["uid"].Value.ToUpper() + "}").ToString();
                    var parentRendering = deviceDef.GetRenderingByUniqueId(parentRenderingId);

                    if (parentRendering == null || renderingsToRemove.Any(r => r.UniqueId == parentRenderingId))
                    {
                        renderingsToRemove.Add(renderingReference);
                    }
                    else if (!CompareParentPlaceholderKey(renderingReference.Placeholder, parentRendering.Placeholder))
                    {
                        var oldPh = renderingReference.Placeholder.Substring(0, renderingReference.Placeholder.LastIndexOf('/'));
                        if (!phKeysToUpdate.ContainsKey(oldPh))
                            phKeysToUpdate.Add(oldPh, parentRendering.Placeholder);
                    }
                }

                for (var i = renderingReferences.Length - 1; i >= 0; i--)
                {
                    var rendering = renderingReferences[i];
                    if (renderingsToRemove.Contains(rendering))
                    {
                        deviceDef.Renderings.Remove(rendering);
                        continue;
                    }

                    var phKeyToAdjust = phKeysToUpdate.Keys.FirstOrDefault(x => rendering.Placeholder.StartsWith(x));
                    if (!string.IsNullOrWhiteSpace(phKeyToAdjust))
                    {
                        rendering.Placeholder = rendering.Placeholder.Replace(phKeyToAdjust,
                                                                              phKeysToUpdate[phKeyToAdjust]);
                    }
                }
            }
            item.UpdateLayoutDefinition(layoutDef);
        }

        private bool CompareParentPlaceholderKey(string renderingPlaceholder, string parentRenderingPlaceholder)
        {
            if (parentRenderingPlaceholder.StartsWith("/"))
                return renderingPlaceholder.StartsWith(parentRenderingPlaceholder);

            return Regex.IsMatch(renderingPlaceholder, @"^/" + parentRenderingPlaceholder + @"/");
        }
    }
}