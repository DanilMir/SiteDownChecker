using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] is not User)
            context.Result = new JsonResult(new {message = "Unauthorized"})
                {StatusCode = StatusCodes.Status401Unauthorized};
    }
}
