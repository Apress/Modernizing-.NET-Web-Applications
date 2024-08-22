using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ModernizationDemo.WebApiCore.Filters;

public class FixNoContentResponseOnVoidActionsFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (
            context.ActionDescriptor is ControllerActionDescriptor controllerAction      // controller action
            && context.Exception is null        // did not end with exception
            && context.Result is EmptyResult    // the result is empty
        )
        {
            // if the action returns void or Task, set status code to 204 instead of 200
            if (controllerAction.MethodInfo.ReturnType == typeof(void)
                || controllerAction.MethodInfo.ReturnType == typeof(Task))
            {
                context.HttpContext.Response.StatusCode = 204;
            }
        }

        base.OnActionExecuted(context);
    }
}