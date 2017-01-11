using Elision.Foundation.Kernel;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class RenderingDatasourceRulesSettings
    {
        public string LocalDatasourceFolderPath => Sitecore.Configuration.Settings.GetSetting(
            "Elision.LocalDatasourceFolderQuery",
            "./_components");

        public string LocalDatasourceTemplateId => Sitecore.Configuration.Settings.GetSetting(
            "Elision.LocalDatasourceFolderTemplateID",
            Templates.ComponentsFolder.TemplateId.ToString());

        public bool LocalDatasourceFolderNesting => Sitecore.Configuration.Settings.GetBoolSetting(
            "Elision.LocalDatasourceFolderNesting",
            true);

        public string WebsiteDatasourceFolderPath => Sitecore.Configuration.Settings.GetSetting(
            "Elision.WebsiteDatasourceFolderQuery",
            "./_components");

        public string WebsiteDatasourceTemplateId => Sitecore.Configuration.Settings.GetSetting(
            "Elision.WebsiteDatasourceFolderTemplateID",
            Templates.ComponentsFolder.TemplateId.ToString());

        public bool WebsiteDatasourceFolderNesting => Sitecore.Configuration.Settings.GetBoolSetting(
            "Elision.WebsiteDatasourceFolderNesting", 
            true);

        public string GlobalDatasourceFolderPath => Sitecore.Configuration.Settings.GetSetting(
            "Elision.GlobalDatasourceFolderQuery",
            "./_components");

        public string GlobalDatasourceTemplateId => Sitecore.Configuration.Settings.GetSetting(
            "Elision.GlobalDatasourceFolderTemplateID",
            Templates.ComponentsFolder.TemplateId.ToString());

        public bool GlobalDatasourceFolderNesting => Sitecore.Configuration.Settings.GetBoolSetting(
            "Elision.GlobalDatasourceFolderNesting", 
            true);
    }
}
