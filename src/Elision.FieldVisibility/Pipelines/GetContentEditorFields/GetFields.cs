using Sitecore.Data.Fields;
using Sitecore.Data.Templates;

namespace Elision.FieldVisibility.Pipelines.GetContentEditorFields
{
    public class GetFields : Sitecore.Shell.Applications.ContentEditor.Pipelines.GetContentEditorFields.GetFields
    {
        protected override bool CanShowField(Field field, TemplateField templateField)
        {
            if (ShowDataFieldsOnly)
            {
                var templateFieldItem = field.Database.GetItem(templateField.ID);
                var hideWithStandardFields =
                    (templateFieldItem?.Fields[Templates.Template_Field.FieldIds.HideWithStandardFields] != null
                     && templateFieldItem.Fields[Templates.Template_Field.FieldIds.HideWithStandardFields].Value == "1");

                if (hideWithStandardFields)
                    return false;
            }
            return base.CanShowField(field, templateField);
        }
    }
}