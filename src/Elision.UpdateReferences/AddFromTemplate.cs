using System;
using Sitecore;
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

            var cache = Sitecore.Caching.CacheManager.FindCacheByName("master[items]");
            cache.RemovePrefix(sourceItem.ID.ToString());
            cache.RemovePrefix(targetItem.ID.ToString());

            EnsureLayoutIsCopied(targetItem.Branch.InnerItem.Children[0], targetItem);

            cache.RemovePrefix(targetItem.ID.ToString());

            _referenceUpdater.UpdateReferences(sourceItem, targetItem);

            cache.RemovePrefix(targetItem.ID.ToString());
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
