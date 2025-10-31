using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers() // dependency injection
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
            }); // it will try to complete the action with invalid data

// this is a check for the token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //puts this in the brackets instead doing it in the controlllers; controllers will want it for authentication
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters() // specifies how the tokens will be validated
        {
            ValidIssuer = "fmi",
            ValidAudience = "front-end",
            //RequireSignedTokens = false, //put later
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                                                        ("!Password123!Password123!Password123")) // that is the key for the third section of the jwt token
            //.net use utf-8 not ASCII
            // the algorithm is not mentiioned, the algorithm is taken from the first section of the token; algorithm is specified in the header of the token
            // hs256 is a cryptographic algorithm that uses a secret key to both sign and verify the token
        };
    });
// returns an instance of authentication builder. addJwtBearer is a method giving the options for solving jwt bearer authentication 

var app = builder.Build();
app.UseHttpsRedirection(); // rejects http requests and wants https
app.UseAuthorization();
app.UseAuthentication(); // check for valid token with the request; configured as a service
// middleware must be before the endpoint resolving
app.MapControllers();

app.Run();
