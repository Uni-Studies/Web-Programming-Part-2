using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // dependency injection
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //puts this in the brackets instead doing it in the controlllers
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidIssuer = "fmi",
//             ValidAudience = "front-end",
//             RequireSignedTokens = false, //put later
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
//                                                         ("!Password123!Password123!Password123"))
//         };
//     });

var app = builder.Build();
app.UseHttpsRedirection(); // rejects http requests and wants https
// app.UseAuthentication(); // check for valid token with the request
// app.UseAuthorization();
app.MapControllers();

app.Run();
