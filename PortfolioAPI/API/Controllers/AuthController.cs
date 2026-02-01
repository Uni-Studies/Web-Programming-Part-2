using System;
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
            var loggedUser = service.Authenticate(model.Username, model.Password);

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

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthTokenRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
            AuthUserServices authService = new AuthUserServices();
            try
            {
                
                var user = authService.Register(model.Username, model.Email, model.Password);

                var authUser = authService.GetById(user.Id);

                TokenServices tokenServices = new TokenServices();
                var token = tokenServices.CreateToken(authUser);

                return Ok(new
                {
                    token,
                    user
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
