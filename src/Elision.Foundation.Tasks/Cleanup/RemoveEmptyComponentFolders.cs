using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace Elision.Foundation.Tasks.Cleanup
{
    public class RemoveEmptyComponentFolders
    {
        public void Remove(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Log.Info($"{nameof(RemoveEmptyComponentFolders)}: Starting to remove empty component folders...", this);
            Assert.IsNotNull(items, "Task items cannot be null");

            if(items.Any())
                Log.Info($"{nameof(RemoveEmptyComponentFolders)}: No components folders passed to task. Aborting...", this);

            var deletedCount = 0;

            using (new SecurityDisabler())
            {
                foreach (var componentFolder in items)
                {
                    if (!componentFolder.HasChildren)
                    {
                        componentFolder.Delete();
                        deletedCount++;
                    }                        
                }
            }

            Log.Info($"{nameof(RemoveEmptyComponentFolders)}: Cleanup complete, deleted {deletedCount} folders.", this);
        }
    }
}