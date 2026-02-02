using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Auth;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("createToken")]
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
                ModelState.AddModelError("Global", "User not found.");
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
        public IActionResult Register([FromForm] AuthRegistrationRequest model)
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
                
                var authUser = authService.Register(model.Username, model.Email, model.Password);
                //var authUser = authService.GetById(user.Id);

                TokenServices tokenServices = new TokenServices();
                var token = tokenServices.CreateToken(authUser);

                return Ok(new
                {
                    token = token
                });
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
        }

        private void PopulateEntity(AuthUser item, AuthRegistrationRequest model)
        {
            item.Email = model.Email ?? item.Email;
            item.Username = model.Username ?? item.Username;
            item.Password = model.Password ?? item.Password;

        }

        [Authorize]
        [HttpPut("editAccount")]
        public IActionResult EditAccount([FromBody] AuthRegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
                
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            AuthUserServices authUserServices = new AuthUserServices();
            var forUpdate = authUserServices.GetById(loggedUserId);

            PopulateEntity(forUpdate, model);
            authUserServices.Save(forUpdate);

            return Ok(ServiceResult<AuthUser>.Success(forUpdate));
        }

        [HttpDelete("deleteAccount")]
        public IActionResult Delete()
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            AuthUserServices service = new AuthUserServices();
            AuthUser forDelete = service.GetById(loggedUserId);
            service.Delete(forDelete);
            
            return Ok(ServiceResult<AuthUser>.Success(forDelete));
        }
    }
}
