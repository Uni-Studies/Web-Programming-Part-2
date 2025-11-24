using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Projects;
using API.Infrastructure.RequestDTOs.Users;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
        public IActionResult Get([FromBody] UserGetRequest model)
        {
            model.Pager = model.Pager ?? new Infrastructure.RequestDTOs.Shared.PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;
            model.OrderBy ??= "Id";
            model.OrderBy = typeof(User).GetProperty(model.OrderBy) != null ? model.OrderBy : "id";
            model.Filter ??= new UserRequest();
            //ModelState.AddModelError("Global", "Empty users list");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");
            UsersServices service = new UsersServices();
            Expression<Func<User, bool>> filter =
                u => 
                (string.IsNullOrEmpty(model.Filter.Username) || u.Username.Contains(model.Filter.Username)) &&
                ( string.IsNullOrEmpty(model.Filter.FirstName) || u.Username.Contains(model.Filter.FirstName)) &&
                (string.IsNullOrEmpty(model.Filter.LastName) || u.Username.Contains(model.Filter.LastName));


        return Ok(service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize));
            // var allUsersResult = service.GetAll();
            // if(allUsersResult.Count == 0)
            // {
            //     return NotFound(ModelState); 
            // }
            // return Ok(allUsersResult);
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
                LastName = model.LastName,
                //Projects = new List<Project>()
            };

            // model.Projects = model.Projects ?? new List<ProjectRequest>();

            // foreach (var pr in model.Projects)
            // {
            //     user.Projects.Add(
            //         new Project
            //         {
            //             Title = pr.Title,
            //             Description = pr.Description
            //         }
            //     );
            // }

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
