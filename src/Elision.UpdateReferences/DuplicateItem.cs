using System;
using Sitecore;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Buckets.Pipelines.UI;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Globalization;
using Sitecore.Web.UI.Sheer;
using Constants = Sitecore.Buckets.Util.Constants;

namespace Elision.UpdateReferences
{
    public class DuplicateItem : ItemDuplicate
    {
        private readonly ITreeReferenceUpdater _referenceUpdater;

        public DuplicateItem(ITreeReferenceUpdater referenceUpdater)
        {
            _referenceUpdater = referenceUpdater;
        }

        public new void Execute(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            var database = Factory.GetDatabase(args.Parameters["database"]);
            Assert.IsNotNull(database, args.Parameters["database"]);

            var sourceItem = database.Items[args.Parameters["id"]];

            var targetItem = Duplicate(args, sourceItem);
            if (targetItem == null || sourceItem == null)
                return;

            _referenceUpdater.UpdateReferences(sourceItem, targetItem);
        }

        public Item Duplicate(ClientPipelineArgs args, Item sourceItem)
        {
            Item result = null;
            if (sourceItem == null)
            {
                SheerResponse.Alert(Translate.Text("Item not found."));
            }
            else
            {
                Item parent = sourceItem.Parent;
                if (parent == null)
                {
                    SheerResponse.Alert(Translate.Text("Cannot duplicate the root item."));
                }
                else if (parent.Access.CanCreate())
                {
                    Log.Audit(this, "Duplicate item: {0}", AuditFormatter.FormatItem(sourceItem));
                    var bucketItemOrSiteRoot = sourceItem.GetParentBucketItemOrSiteRoot();
                    if (BucketManager.IsBucket(bucketItemOrSiteRoot) && BucketManager.IsBucketable(sourceItem))
                    {
                        if (!EventDisabler.IsActive)
                        {
                            EventResult eventResult = Event.RaiseEvent("item:bucketing:duplicating", args, this);
                            if (eventResult != null && eventResult.Cancel)
                            {
                                Log.Info(string.Format("Event {0} was cancelled", "item:bucketing:duplicating"), this);
                                args.AbortPipeline();
                                return null;
                            }
                        }
                        result = Context.Workflow.DuplicateItem(sourceItem, args.Parameters["name"]);
                        Item destination = CreateAndReturnBucketFolderDestination(bucketItemOrSiteRoot, DateUtil.ToUniversalTime(DateTime.Now), sourceItem);
                        if (!IsBucketTemplateCheck(sourceItem))
                            destination = bucketItemOrSiteRoot;

                        ItemManager.MoveItem(result, destination);

                        if (!EventDisabler.IsActive)
                            Event.RaiseEvent("item:bucketing:duplicated", args, this);
                    }
                    else
                        result = Context.Workflow.DuplicateItem(sourceItem, args.Parameters["name"]);
                }
                else
                {
                    SheerResponse.Alert(Translate.Text("You do not have permission to duplicate \"{0}\".", sourceItem.DisplayName));
                }
            }
            args.AbortPipeline();
            return result;
        }

        internal static Item CreateAndReturnBucketFolderDestination(Item topParent, DateTime childItemCreationDateTime, Item item)
        {
            return CreateAndReturnBucketFolderDestination(topParent, childItemCreationDateTime, item.ID, item.Name, item.TemplateID);
        }
        internal static Item CreateAndReturnBucketFolderDestination(Item topParent, DateTime childItemCreationDateTime, ID itemId, string itemName, ID templateId)
        {
            return BucketManager.Provider.CreateAndReturnBucketFolderDestination(topParent, childItemCreationDateTime, itemId, itemName, templateId);
        }

        internal static bool IsBucketTemplateCheck(Item item)
        {
            if (item != null)
            {
                if (item.Fields[Constants.IsBucket] != null)
                    return item.Fields[Constants.BucketableField].Value.Equals("1");
                if (item.Paths.FullPath.StartsWith("/sitecore/templates"))
                {
                    TemplateItem templateItem1 = item.Children[0] != null ? item.Children[0].Template : null;
                    if (templateItem1 != null)
                    {
                        TemplateItem templateItem2 = new TemplateItem(templateItem1);
                        if (templateItem1.StandardValues != null && templateItem2.StandardValues[Constants.BucketableField] != null)
                            return templateItem2.StandardValues[Constants.BucketableField].Equals("1");
                    }
                }
            }
            return false;
        }
    }
}
