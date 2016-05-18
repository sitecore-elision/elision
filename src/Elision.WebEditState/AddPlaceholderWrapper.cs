namespace Elision.WebEditState
{
    public class AddPlaceholderWrapper : Sitecore.Mvc.ExperienceEditor.Pipelines.Response.RenderPlaceholder.AddWrapper
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderPlaceholder.RenderPlaceholderArgs args)
        {
            if (WebEditStateSwitcher.CurrentValue == WebEditState.Disabled)
                return;

            base.Process(args);
        }
    }
}
