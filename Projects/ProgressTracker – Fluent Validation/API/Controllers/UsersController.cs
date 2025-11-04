using System.Collections.Generic;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Users;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // if more authentication methods. then specify [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(AuthenticationSchemes = "Bearer")] // attribute of the framework which works with the configured frameworks; 
    // tells that this controller needs authentication with bearer tokens
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //ModelState.AddModelError("Global", "Empty users list");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");
            UsersServices service = new UsersServices();
            var allUsersResult = service.GetAll();
            if(allUsersResult.Count == 0)
            {
                return NotFound(ModelState); 
            }
            return Ok(allUsersResult);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "User not found");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");   
            UsersServices service = new UsersServices();
            var userResult = service.GetById(id);
            if (userResult is null)
            {
                return NotFound(ModelState);
            }
            return Ok(userResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserRequest model)
        {
            UsersServices service = new UsersServices();
            User user = new User
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            service.Save(user);
            
            // return Ok(model);
            return Ok(model);
        }

        [HttpDelete] // for single parameter
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "User not found");
            UsersServices service = new UsersServices();
            var forDeleteResult = service.GetById(id);
            if (forDeleteResult is null)
                // throw new System.Exception("User not found");
                return NotFound(ModelState);

            service.Delete(forDeleteResult);

            return Ok(forDeleteResult);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] UserRequest model)
        {
            UsersServices service = new UsersServices();
            ModelState.AddModelError("Global", "User not found");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var forUpdateResult = service.GetById(id);
            if (forUpdateResult is null)
                //throw new System.Exception("User not found");
                return NotFound(ModelState);

            User forUpdate = forUpdateResult;  
            forUpdate.Username = model.Username;
            forUpdate.Password = model.Password;
            forUpdate.FirstName = model.FirstName;
            forUpdate.LastName = model.LastName;
            
            service.Save(forUpdate);
            // return Ok(model);
            return Ok(model);
        }
    }
}
