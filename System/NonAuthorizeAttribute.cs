using Microsoft.AspNetCore.Mvc.Filters;
namespace Ndknitor.System;
public class NonAuthorizedAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            var res = new
            {
                Message = "User is authorized"
            };
            await context.HttpContext.Response.WriteAsync(res.ToJson());
        }
        else
        {
            await next();
        }
    }
}