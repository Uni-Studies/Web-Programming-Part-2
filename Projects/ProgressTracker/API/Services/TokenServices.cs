using System;
using Common.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace API.Services;

public class TokenServices
{
    public string CreateToken(User user)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("loggedUserId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                                                        ("!Password123!Password123!Password123"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // the algorithm for the third section of the token

        // initialize a token handler with a specific constructor
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "fmi", // the creator of the token
            audience: "front-end",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: cred // null before
        );

        // string a = new JwtSecurityTokenHandler();
        //string tokenData = a.WriteToken(token);
        
        string tokenData = new JwtSecurityTokenHandler()
                                        .WriteToken(token);
        return tokenData;
    }
}
