using Sitecore.Common;

namespace Elision.WebEditState
{
    public class WebEditDisabler : WebEditStateSwitcher
    {
        public WebEditDisabler() : base(WebEditState.Disabled)
        {
        }
    }

    public class WebEditStateSwitcher : Switcher<WebEditState>
    {
        public WebEditStateSwitcher(WebEditState state) : base(state)
        {
        }
    }

    public enum WebEditState
    {
        Default,
        Disabled,
        Enabled,
    }
}
