using Elision.Foundation.Kernel;

namespace Elision.Foundation.SiteResources.Pipelines.GetSiteResources
{
    public class GetRawSiteAndPageResources : IGetSiteResourcesPipelineHandler
    {
        public void Process(GetSiteResourcesArgs args)
        {
            var siteFieldName = "";
            var pageFieldName = "";
            if (args.ResourceLocationId == OptionLists.ResourceLocations.Head)
            {
                siteFieldName = Templates._SiteScripts.FieldNames.SiteHeadScript;
                pageFieldName = Templates._PageScripts.FieldNames.PageHeadScript;
            }
            else if (args.ResourceLocationId == OptionLists.ResourceLocations.Body_Top)
            {
                siteFieldName = Templates._SiteScripts.FieldNames.SiteBodyTopScript;
                pageFieldName = Templates._PageScripts.FieldNames.PageBodyTopScript;
            }
            else if (args.ResourceLocationId == OptionLists.ResourceLocations.Body_Bottom)
            {
                siteFieldName = Templates._SiteScripts.FieldNames.SiteFootScript;
                pageFieldName = Templates._PageScripts.FieldNames.PageFootScript;
            }

            if (!string.IsNullOrWhiteSpace(siteFieldName))
            {
                var siteScripts = (args.ContextItem ?? Sitecore.Context.Item).GetInheritedFieldValue(siteFieldName);
                if (!string.IsNullOrWhiteSpace(siteScripts))
                    args.Results.Add(new SiteResource("SiteRawScripts", siteScripts));
            }
            if (args.ContextItem != null && !string.IsNullOrWhiteSpace(pageFieldName))
            {
                var pageScripts = args.ContextItem[pageFieldName];
                if (!string.IsNullOrWhiteSpace(pageScripts))
                    args.Results.Add(new SiteResource("PageRawScripts", pageScripts));
            }
        }
    }
}
