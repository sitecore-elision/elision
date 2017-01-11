using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Pipelines;

namespace Elision.Foundation.UpdateReferences
{
    public class CopyOrCloneItem : Sitecore.Shell.Framework.Pipelines.CopyItems
    {
        private readonly ITreeReferenceUpdater _referenceUpdater;

        public CopyOrCloneItem(ITreeReferenceUpdater referenceUpdater)
        {
            _referenceUpdater = referenceUpdater;
        }

        public virtual void ProcessFieldValues(CopyItemsArgs args)
        {
            var sourceRoot = GetItems(args).FirstOrDefault();
            Assert.ArgumentNotNull(sourceRoot, "sourceRoot");

            var targetItem = args.Copies.FirstOrDefault();
            Assert.ArgumentNotNull(targetItem, "targetItem");

            _referenceUpdater.UpdateReferences(sourceRoot, targetItem);
        }
    }
}
