using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context, SiteDownContext dataContext)
    {
        var token = context.Request.Cookies["Authorization"];
        if (token is not null && token != string.Empty) 
            await AttachAccountToContext(context, dataContext, token);

        await _next(context);
    }

    private async Task AttachAccountToContext(HttpContext context, SiteDownContext dataContext, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Config.JwtKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = validatedToken as JwtSecurityToken;
        var accountId = int.Parse(jwtToken!.Claims.First(x => x.Type is "id").Value);

        context.Items["User"] = await dataContext.Users.FindAsync(accountId);
    }
}
