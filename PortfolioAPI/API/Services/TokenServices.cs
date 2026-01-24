using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenServices
{
    public string CreateToken(AuthUser user)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("loggedUserId", user.Id.ToString())
        };

        var key =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourSuperSecretKeyHere"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "PortfolioAPI",
            audience: "PortfolioAPIClient",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials
        );

        string tokenData = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenData;
    }
}
