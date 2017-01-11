using System;
using System.Collections.Generic;
using System.Linq;
using Elision.Foundation.Kernel;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web;

namespace Elision.Foundation.FieldEditor
{
    public class GenerateFieldEditorUrl : PipelineProcessorRequest<ItemContext>
    {
        public string GenerateUrl()
        {
            var parameters = WebUtil.ParseQueryString(RequestContext.Argument);
            var fieldeditorOption = new FieldEditorOptions(CreateFieldDescriptors(parameters["fieldnames"], parameters["datasource"]));

            if (!string.IsNullOrWhiteSpace(parameters["dialogtitle"]))
                fieldeditorOption.DialogTitle = parameters["dialogtitle"];

            bool boolValue;
            fieldeditorOption.PreserveSections = !bool.TryParse(parameters["preservesections"], out boolValue) || boolValue;

            fieldeditorOption.SaveItem = true;
            return fieldeditorOption.ToUrlString().ToString();
        }

        private IEnumerable<FieldDescriptor> CreateFieldDescriptors(string fields, string datasourceString)
        {
            var db = Factory.GetDatabase(RequestContext.Database);
            var item = db.ResolveDatasource(datasourceString, RequestContext.Item)
                       ?? RequestContext.Item;

            var fieldString = new ListString(fields);
            return new ListString(fieldString)
                .Where(x => item.Fields[x] != null)
                .Select(field => new FieldDescriptor(item, field))
                .ToList();
        }

        public override PipelineProcessorResponseValue ProcessRequest()
        {
            var response = new PipelineProcessorResponseValue();
            try
            {
                response.Value = GenerateUrl();
            }
            catch (Exception ex)
            {
                response.AbortMessage = ex.ToString();
            }
            return response;
        }
    }
}