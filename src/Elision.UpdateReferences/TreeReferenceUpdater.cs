using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;
using Sitecore.SecurityModel;

namespace Elision.Foundation.UpdateReferences
{
    public interface ITreeReferenceUpdater
    {
        void UpdateReferences(Item sourceRootItem, Item targetRootItem);
    }

    public class TreeReferenceUpdater : ITreeReferenceUpdater
    {
        public void UpdateReferences(Item sourceRootItem, Item targetRootItem)
        {
            Assert.ArgumentNotNull(sourceRootItem, "sourceRootItem");
            Assert.ArgumentNotNull(targetRootItem, "targetRootItem");

            var itemMap = BuildMatchingItemMap(sourceRootItem, sourceRootItem, targetRootItem)
                .Concat(new[] {new EqualItems {Source = sourceRootItem, Dest = targetRootItem}})
                .Distinct(new GenericEqualityComparer<EqualItems>((a, b) => a.Source.Equals(b.Source) && a.Dest.Equals(b.Dest), x => x.GetHashCode()))
                .ToArray();

            FixReferences(targetRootItem, itemMap);
        }

        private IEnumerable<EqualItems> BuildMatchingItemMap(Item item, Item sourceRootItem, Item targetRootItem)
        {
            if (item == null)
                yield break;

            foreach (Item child in item.GetChildren(ChildListOptions.IgnoreSecurity | ChildListOptions.SkipSorting))
            {
                var targetPath = child.Paths.Path.Replace(sourceRootItem.Paths.Path, targetRootItem.Paths.Path);
                var matchingTargetItem = targetRootItem.Database.GetItem(targetPath);

                if (matchingTargetItem == null)
                    continue;

                yield return new EqualItems() { Source = child, Dest = matchingTargetItem };

                foreach (var childMatchingItem in BuildMatchingItemMap(child, sourceRootItem, targetRootItem))
                {
                    yield return childMatchingItem;
                }
            }
        }

        private void FixReferences(Item item, EqualItems[] matchingItemMap)
        {
            if (item == null) return;
            var fields = GetFieldsToProcess(item);
            foreach (var field in fields)
            {
                foreach (var itemVersion in item.Versions.GetVersions(true))
                {
                    var itemVersionField = itemVersion.Fields[field.ID];
                    ProcessField(itemVersionField, matchingItemMap);
                }
            }

            if (!item.HasChildren)
                return;

            foreach (Item child in item.GetChildren(ChildListOptions.IgnoreSecurity | ChildListOptions.SkipSorting))
                FixReferences(child, matchingItemMap);
        }

        //Reads all field of given item including Clone items.
        private IEnumerable<Field> GetFieldsToProcess(Item item)
        {
            item.Fields.ReadAll();
            return item.Fields.Where(ShouldProcessField).ToArray();
        }

        protected bool ShouldProcessField(Field field)
        {
            if (field == null) return false;

            return field.ID == FieldIDs.LayoutField
                   || field.ID == FieldIDs.FinalLayoutField
                   || !field.Name.StartsWith("__");
        }

        private void ProcessField(Field field, EqualItems[] matchingItemMap)
        {
            string initialValue;
            if (field.ID == Sitecore.FieldIDs.LayoutField || field.ID == Sitecore.FieldIDs.FinalLayoutField)
                initialValue = LayoutField.GetFieldValue(field);
            else
                initialValue = field.GetValue(true, true);

            if (string.IsNullOrEmpty(initialValue))
                return;

            var value = new StringBuilder(initialValue);
            foreach (var r in matchingItemMap)
            {
                value = value.Replace(r.Source.ID.Guid.ToString("D").ToUpper(), r.Dest.ID.Guid.ToString("D").ToUpper());
                value = value.Replace(r.Source.ID.Guid.ToString("D").ToLower(), r.Dest.ID.Guid.ToString("D").ToLower());
                value = value.Replace(r.Source.ID.Guid.ToString("N").ToUpper(), r.Dest.ID.Guid.ToString("N").ToUpper());
                value = value.Replace(r.Source.ID.Guid.ToString("N").ToLower(), r.Dest.ID.Guid.ToString("N").ToLower());
                value = value.Replace(r.Source.Paths.Path, r.Dest.Paths.Path);
                value = value.Replace(r.Source.Paths.Path.ToLower(), r.Dest.Paths.Path.ToLower(), true);
                if (!r.Source.Paths.IsContentItem)
                    continue;

                value.Replace(r.Source.Paths.ContentPath, r.Dest.Paths.ContentPath);
                value.Replace(r.Source.Paths.ContentPath.ToLower(), r.Dest.Paths.ContentPath.ToLower(), true);
            }

            if (field.ID == FieldIDs.LayoutField || field.ID == FieldIDs.FinalLayoutField)
            {
                using (new Sitecore.Data.Events.EventDisabler())
                using (new EditContext(field.Item, SecurityCheck.Disable))
                {
                    LayoutField.SetFieldValue(field, value.ToString());
                }
            }

            else
            {
                UpdateFieldValue(field, initialValue, value);
            }                
        }

        protected void UpdateFieldValue(Field field, string initialValue, StringBuilder value)
        {
            if (initialValue.Equals(value.ToString()))
                return;

            using (new Sitecore.Data.Events.EventDisabler())
            using (new EditContext(field.Item, SecurityCheck.Disable))
            {
                field.Value = value.ToString();
            }
        }
    }
}
