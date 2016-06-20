using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.SiteResources.Pipelines.GetSiteResources
{
    public class GetSiteResourcesArgs : PipelineArgs
    {
        public readonly List<SiteResource> Results = new List<SiteResource>();

        public ID ResourceLocationId { get; set; }
        public Item ContextItem { get; set; }
        public ID DeviceId { get; set; }

        public GetSiteResourcesArgs(Item contextPage, ID resourceLocationId, ID deviceId)
        {
            ContextItem = contextPage;
            ResourceLocationId = resourceLocationId;
            DeviceId = deviceId;
        }
    }
}
