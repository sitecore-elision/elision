using System;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Layouts;
using Sitecore.Web;

namespace Elision.Foundation.Kernel
{
    public static class ItemInformationExtensions
    {
        public static bool InheritsFrom(this Item item, ID templateId)
        {
            return item.TemplateID == templateId
                   || item.Template.InheritsFrom(templateId);
        }

        public static bool InheritsFrom(this TemplateItem template, ID templateId)
        {
            return template.ID == templateId
                   || template.BaseTemplates
                              .Where(x => x.ID != template.ID)
                              .Any(x => x.InheritsFrom(templateId));
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateId)
        {
            var currentItem = item;
            while (currentItem != null)
            {
                if (currentItem.InheritsFrom(templateId))
                    return currentItem;
                currentItem = currentItem.Parent;
            }
            return null;
        }

        public static string GetValue(this FieldCollection fields, ID fieldId)
        {
            return fields[fieldId]?.Value;
        }

        public static Field GetInheritedField(this Item item, string fieldName, bool skipEmptyFields = false)
        {
            while (true)
            {
                if (item == null)
                    return null;

                var field = item.Fields[fieldName];
                if (field != null && skipEmptyFields && string.IsNullOrWhiteSpace(field.Value))
                    field = null;

                if (field != null) return field;
                item = item.Parent;
            }
        }

        public static string GetInheritedFieldValue(this Item item, string fieldName, bool skipEmptyValues = false)
        {
            var field = item.GetInheritedField(fieldName, skipEmptyValues);
            return field?.Value;
        }

        public static SiteInfo GetSite(this Item item)
        {
            var siteInfoList = Sitecore.Sites.SiteContextFactory.Sites
                .Where(site => !string.IsNullOrWhiteSpace(site.Database))
                .Where(site =>
                    item.Database.Name.Equals("core", StringComparison.CurrentCultureIgnoreCase) ||
                    !site.Database.Equals("core", StringComparison.CurrentCultureIgnoreCase))
                .Where(x => !(x.PhysicalFolder ?? "").StartsWith("/sitecore", StringComparison.CurrentCultureIgnoreCase))
                .OrderByDescending(x => (x.RootPath + x.StartItem).Length);

            var itemPath = item.Paths.FullPath;

            return 
                siteInfoList.FirstOrDefault(site => itemPath.StartsWith(site.RootPath + site.StartItem, StringComparison.InvariantCultureIgnoreCase)) 
                ?? siteInfoList.FirstOrDefault(site => itemPath.StartsWith(site.RootPath, StringComparison.CurrentCultureIgnoreCase));
        }

        public static LayoutDefinition GetLayoutDefinition(this Item item)
        {
            var layoutField = item.Fields[SitecoreIds.FinalLayoutField];
            return LayoutDefinition.Parse(LayoutField.GetFieldValue(layoutField));
        }

        public static void UpdateLayoutDefinition(this Item item, LayoutDefinition layoutDefinition)
        {
            var layoutField = item.Fields[SitecoreIds.FinalLayoutField];
            LayoutField.SetFieldValue(layoutField, layoutDefinition.ToXml());
        }

        public static bool HasPresentation(this Item item, DeviceItem device)
        {
            if (item == null || device == null) return false;
            if (item.Visualization.Layout == null) return false;

            return item.Visualization.GetLayout(device) != null;
        }

        public static bool HasLanguage(this Item item, string languageName)
        {
            return ItemManager.GetVersions(item, LanguageManager.GetLanguage(languageName, item.Database)).Count > 0;
        }

        public static bool HasLanguage(this Item item, Language language)
        {
            return ItemManager.GetVersions(item, language).Count > 0;
        }

        public static bool HasContextLanguage(this Item item)
        {
            var latestVersion = item.Versions.GetLatestVersion();
            return latestVersion?.Versions.Count > 0;
        }
    }
}
