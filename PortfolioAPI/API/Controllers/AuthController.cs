using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Auth;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken([FromForm] AuthTokenRequest model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }

            AuthUserServices service = new AuthUserServices();
            AuthUser loggedUser = service.GetAll().FirstOrDefault(
                u => u.Username == model.Username && u.Password == model.Password
            );

            if(loggedUser == null)
            {
                ModelState.AddModelError("Global", "Invalid username or password.");
                return Unauthorized(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }

            TokenServices tokenService = new TokenServices();
            string token = tokenService.CreateToken(loggedUser);
            return Ok(new
            {
                token = token
            }
            );
        }
    }
}
