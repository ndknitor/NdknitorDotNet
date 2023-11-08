using Microsoft.AspNetCore.Mvc.Filters;
namespace Ndknitor.System;
public class NonAuthorizedAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.HttpContext.Response.CompleteAsync();
        }
        else
        {
            await next();
        }
    }
}