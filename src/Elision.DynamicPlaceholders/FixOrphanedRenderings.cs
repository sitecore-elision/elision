using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Sitecore;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;

namespace Elision.DynamicPlaceholders
{
    public class FixOrphanedRenderings
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;
            if (item == null)
                return;

            var device = Context.Device;
            if (device == null)
                return;

            var regex = new Regex(GetDynamicKeyAllowedRenderings.DynamicKeyPattern);
            var renderingReferences = item.Visualization.GetRenderings(device, false);
            foreach (var renderingReference in renderingReferences)
            {
                var key = renderingReference.Placeholder;
                var match = regex.Match(renderingReference.Placeholder);
                if (!match.Success || match.Groups.Count <= 0)
                    continue;

                var parentRenderingId = "{" + key.Substring(key.Length - 36, 36).ToUpper() + "}";
                if (renderingReferences.All(r => r.UniqueId.ToUpper() != parentRenderingId))
                    RemovedRenderingReference(item, renderingReference.UniqueId);
            }
        }

        public void RemovedRenderingReference(Item item, string renderingReferenceUid)
        {
            var layoutFieldId = FieldIDs.LayoutField;
            var document = new XmlDocument();

            document.LoadXml(item[layoutFieldId]);

            var node = document.SelectSingleNode($"//r[@uid='{renderingReferenceUid}']");

            if (node?.ParentNode == null) return;

            node.ParentNode.RemoveChild(node);

            using (new EventDisabler())
            using (new EditContext(item))
            {
                item[layoutFieldId] = document.OuterXml;
            }
        }
    }
}