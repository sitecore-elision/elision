using System.Linq;
using Elision.Foundation.Kernel;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Elision.Foundation.Themes
{
    public interface IThemeRetriever
    {
        Item GetThemeFromContextItem(Item contextItem);
        string GetThemeResources(Item theme, ID deviceId, ID resourceLocationId);
    }

    public class ThemeRetriever : IThemeRetriever
    {
        public Item GetThemeFromContextItem(Item contextItem)
        {
            var themableAncestor = new[] {contextItem}
                .Union(contextItem.Axes.GetAncestors())
                .FirstOrDefault(x => x.InheritsFrom(Templates._Themable.TemplateId) && !string.IsNullOrWhiteSpace(x[Templates._Themable.FieldIds.Theme]));

            if (themableAncestor == null) return null;

            return contextItem.Database.ResolveDatasource(themableAncestor[Templates._Themable.FieldIds.Theme], themableAncestor);
        }

        public string GetThemeResources(Item theme, ID deviceId, ID resourceLocationId)
        {
            return GetThemeStyles(theme, deviceId, resourceLocationId)
                   + "\r\n"
                   + GetThemeScripts(theme, deviceId, resourceLocationId);
        }

        public string GetThemeScripts(Item theme, ID deviceId, ID resourceLocationId)
        {
            var device = deviceId.ToString();
            var location = resourceLocationId.ToString();

            var themeScriptsFolder = theme.Axes.SelectSingleItem("./Scripts");

            return GetThemeResourceCode(themeScriptsFolder, device, location,
                                        "<script type=\"text/javascript\" src=\"{0}\"></script>");
        }

        public string GetThemeStyles(Item theme, ID deviceId, ID resourceLocationId)
        {
            var device = deviceId.ToString();
            var location = resourceLocationId.ToString();

            var themeStylesFolder = theme.Axes.SelectSingleItem("./Stylesheets");
            if (themeStylesFolder == null)
                return "";

            return GetThemeResourceCode(themeStylesFolder, device, location,
                                        "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />");
        }

        protected virtual string GetThemeResourceCode(Item resourceFolder, string device, string location, string linkFormat)
        {
            if (resourceFolder == null)
                return "";

            var scriptsByDevice = resourceFolder
                .GetChildren(ChildListOptions.IgnoreSecurity)
                .Where(x => string.IsNullOrWhiteSpace(x[Templates._ThemeResource.FieldIds.SupportedDevices])
                            || x[Templates._ThemeResource.FieldIds.SupportedDevices].Contains(device))
                .Where(MatchesCurrentPageMode);

            var scriptsForOutputLocation = scriptsByDevice.Where(x => x[Templates._ThemeResource.FieldIds.ResourceLocation] == location);

            var scriptCodes = scriptsForOutputLocation
                .Select(x => x.InheritsFrom(Templates.ThemeLinkedResource.TemplateId)
                                 ? string.Format(linkFormat, GetResourceUrl(x.Fields[Templates.ThemeLinkedResource.FieldIds.ResourceLink]))
                                 : x[Templates.ThemeEmbeddedResource.FieldIds.ResourceCode]
                );

            return string.Join("\r\n", scriptCodes);
        }

        protected virtual bool MatchesCurrentPageMode(Item resourceItem)
        {
            var selectedModes = resourceItem.GetLinkedItems(Templates._ThemeResource.FieldNames.PageModes).ToArray();
            if (!selectedModes.Any()) return true;

            foreach (var selectedMode in selectedModes.Select(x => x.Name))
            {
                if (selectedMode == "IsNormal" && Sitecore.Context.PageMode.IsNormal)
                    return true;
                if (selectedMode == "IsPreview" && Sitecore.Context.PageMode.IsPreview)
                    return true;
                if ((selectedMode == "IsPageEditor" || selectedMode == "IsExperienceEditor") && Sitecore.Context.PageMode.IsExperienceEditor)
                    return true;
                if ((selectedMode == "IsPageEditorEditing" || selectedMode == "IsExperienceEditorEditing") && Sitecore.Context.PageMode.IsExperienceEditorEditing)
                    return true;
            }
            return false;
        }

        protected virtual string GetResourceUrl(LinkField linkField)
        {
            return string.IsNullOrWhiteSpace(linkField?.Value) ? null : linkField.GetFriendlyUrl();
        }
    }
}
