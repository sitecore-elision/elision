using System;
using Elision.Foundation.Kernel;
using Sitecore.Data;
using Sitecore.Pipelines.GetLookupSourceItems;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Text;
using Sitecore.Web;

namespace Elision.Foundation.LookupSourceItems.Pipelines.GetLookupSourceItems
{
    public class FixQueryRootForRenderingParameterTemplates
    {
        public void Process(GetLookupSourceItemsArgs args)
        {
            if (!args.Item.InheritsFrom(SitecoreIds.StandardRenderingParametersTemplateId))
                return;

            var url = WebUtil.GetQueryString();
            if (string.IsNullOrWhiteSpace(url))
                return;

			try
			{
				var parameters = FieldEditorOptions.Parse(new UrlString(url)).Parameters;

				var currentItemUri = parameters["contentitem"];
				if (string.IsNullOrEmpty(currentItemUri))
					return;

				var contentItemUri = new ItemUri(currentItemUri);
				var contextItem = Database.GetItem(contentItemUri);

				args.Item = contextItem;	
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
			}
        }
    }
}