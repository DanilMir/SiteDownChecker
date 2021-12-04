using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SiteDownChecker.Business.DataBase;
using SiteDownChecker.Business.Models;

namespace SiteDownChecker.API.Authentication;

public readonly struct JwtAuthManager : IJwtAuthManager
{
    private readonly string _tokenKey;

    public JwtAuthManager(string tokenKey) =>
        _tokenKey = tokenKey;

    public string GetToken(string id)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", id)}),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public void Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachAccountToContext(context, token);
    }

    private void AttachAccountToContext(HttpContext context, string token)
    {
        var key = Encoding.ASCII.GetBytes(_tokenKey);
        new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken) validatedToken;
        var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        context.Items["Account"] = Serializer<User>.DeserializeFromId(userId);
    }
}
