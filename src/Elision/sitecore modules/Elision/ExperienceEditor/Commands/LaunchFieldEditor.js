define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.LaunchFieldEditor =
    {
        canExecute: function (context) {
            // Determines whether command is disabled or enabled.
            //debugger;
            return true;
        },
        execute: function (context) {
            context.currentContext.argument = context.button.viewModel.$el[0].title
                || context.button.viewModel.$el[0].accessKey;

            ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.GenerateFieldEditorUrl", function (response) {
                var dialogUrl = response.responseValue.value;
                var dialogFeatures = "";// "dialogHeight: 600px;dialogWidth: 800px;";
                ExperienceEditor.Dialogs.showModalDialog(dialogUrl, '', dialogFeatures, null);
            }).execute(context);
        }
    };
});
