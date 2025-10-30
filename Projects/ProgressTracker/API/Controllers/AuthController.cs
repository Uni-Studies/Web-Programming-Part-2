using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.RequestDTOs.Auth;
using API.Services;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Common;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken([FromForm] AuthTokenRequest model) //[FromForm]string username, [FromForm]string password
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ServiceResultExtensions<List<Error>>.Failure(null, ModelState)
                );
            }
            // if (!ModelState.IsValid)
            // {
            //     var errors = new List<Error>();
            //     foreach(var kvp in ModelState)
            //     {
            //         errors.Add(new Error
            //         {
            //             Key = kvp.Key,
            //             Messages = kvp.Value.Errors
            //                         .Select(e => e.ErrorMessage)
            //                         .ToList()
            //         });
            //     }
            //     return BadRequest(
            //         ServiceResult<List<Error>>.Failure(errors, null)
            //     );
            // }
            
            UsersServices service = new UsersServices();
            User loggedUser = service.GetAll()
                                        .FirstOrDefault(u =>
                                            u.Username == model.Username &&
                                            u.Password == model.Password);
            if (loggedUser == null)
            {
                ModelState.AddModelError("Global", "Invalid username or password");
                
                return Unauthorized(
                    ServiceResultExtensions<List<Error>>.Failure(null, ModelState)
                );
                // var errors = new List<Error>();
                // foreach(var kvp in ModelState)
                // {
                //     errors.Add(new Error
                //     {
                //         Key = kvp.Key,
                //         Messages = kvp.Value.Errors
                //                     .Select(e => e.ErrorMessage)
                //                     .ToList()
                //     });
                // }
                // return BadRequest(
                //     ServiceResult<List<Error>>.Failure(errors, null)
                // );
                //return Unauthorized(ModelState);
                //return Unauthorized();
            }    

            TokenServices tokenService = new TokenServices();
            string token = tokenService.CreateToken(loggedUser);

            return Ok(new
            {
                token = token
            });
        }
    }
}
