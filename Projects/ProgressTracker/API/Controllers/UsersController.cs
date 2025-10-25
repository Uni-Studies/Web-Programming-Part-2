using System.Security.Claims;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // if more authentication methods. then specify [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");   
            UsersServices service = new UsersServices();
            return Ok(service.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] User model)
        {
            UsersServices service = new UsersServices();
            service.Save(model);
            
            return Ok(model);
        }

        [HttpDelete] // for single parameter
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            UsersServices service = new UsersServices();
            User forDelete = service.GetById(id);
            if (forDelete == null)
                throw new System.Exception("User not found");
            service.Delete(forDelete);
            return Ok(forDelete);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User model)
        {
            UsersServices service = new UsersServices();
            User forUpdate = service.GetById(id);
            if (forUpdate == null)
                throw new System.Exception("User not found");

            forUpdate.Username = model.Username;
            forUpdate.Password = model.Password;
            forUpdate.FirstName = model.FirstName;
            forUpdate.LastName = model.LastName;
            service.Save(forUpdate);
            
            return Ok(forUpdate);
        }
    }
}
