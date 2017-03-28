using System;
using System.Linq;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.SecurityModel;

namespace Elision.Foundation.UpdateReferences
{
    public class AddFromTemplate
    {
        private readonly ITreeReferenceUpdater _referenceUpdater;

        public AddFromTemplate(ITreeReferenceUpdater referenceUpdater)
        {
            _referenceUpdater = referenceUpdater;
        }

        public void OnItemAdded(object sender, EventArgs args)
        {
            var targetItem = Event.ExtractParameter(args, 0) as Item;
            if (targetItem?.Branch?.InnerItem?.Children == null || targetItem.Branch.InnerItem.Children.Count == 0)
                return;

            var sourceItem = targetItem.Branch.InnerItem.Children[0];

            var cache = CacheManager.GetItemCache(Sitecore.Configuration.Factory.GetDatabase("master"));
            if (cache == null)
                return;

            UpdateItemFromSource(cache, sourceItem, targetItem);

            var sourceItems = sourceItem.Axes.GetDescendants().Where(x => x?.Visualization.Layout != null).ToArray();
            var targetItems = targetItem.Axes.GetDescendants().Where(x => x?.Visualization.Layout != null);

            foreach (var item in targetItems)
            {
                var relatedSourceItem = sourceItems.FirstOrDefault(x => x.Name == item.Name && x[FieldIDs.FinalLayoutField] == item[FieldIDs.FinalLayoutField]);
                if (relatedSourceItem == null)
                    continue;
                UpdateItemFromSource(cache, relatedSourceItem, item);
            }
        }

        private void UpdateItemFromSource(ItemCache cache, Item sourceItem, Item targetItem)
        {
            cache.RemoveItem(sourceItem.ID);
            cache.RemoveItem(targetItem.ID);

            EnsureLayoutIsCopied(sourceItem, targetItem);

            cache.RemoveItem(targetItem.ID);

            _referenceUpdater.UpdateReferences(sourceItem, targetItem, targetItem.Branch?.InnerItem ?? sourceItem);

            cache.RemoveItem(targetItem.ID);
        }

        private void EnsureLayoutIsCopied(Item sourceItem, Item targetItem)
        {
            var sourceLayout = sourceItem.Fields[FieldIDs.LayoutField].GetValue(false, false, false, false, true);
            var sourceFinalLayout = sourceItem.Fields[FieldIDs.FinalLayoutField].GetValue(false, false, false, false, true);

            using (new Sitecore.Data.Events.EventDisabler())
            using (new EditContext(targetItem, SecurityCheck.Disable))
            {
                targetItem[FieldIDs.LayoutField] = sourceLayout;
                targetItem[FieldIDs.FinalLayoutField] = sourceFinalLayout;
            }
        }
    }
}
