using System.Reflection;
using System.Web.Mvc;

namespace Elision.Forms
{
    public class ValidateFormIdentifier : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var controller = controllerContext.HttpContext.Request.Form["formController"];
            var action = controllerContext.HttpContext.Request.Form["formAction"];

            return !string.IsNullOrWhiteSpace(controller)
                    && !string.IsNullOrWhiteSpace(action)
                    && controller == controllerContext.Controller.GetType().Name
                    && methodInfo.Name == action;
        }
    }

}
