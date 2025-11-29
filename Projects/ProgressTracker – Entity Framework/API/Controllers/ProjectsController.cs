using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Projects;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        // 1. In Projects folder create ProjectsGetRequest, ProjectsGetFilterRequest
        // 2. In the ProjectController implement GET with filtering, paging 
        [HttpGet]
        public IActionResult Get([FromBody] ProjectsGetRequest model)
        {
            //ModelState.AddModelError("Global", "Empty users list");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");
            
            model.Pager = model.Pager ?? new Infrastructure.RequestDTOs.Shared.PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;
            model.OrderBy ??= "Id";
            model.OrderBy = typeof(Project).GetProperty(model.OrderBy) != null ? model.OrderBy : "id";
            model.Filter ??= new ProjectsGetFilterRequest();
            
            ProjectServices service = new ProjectServices();

            int loggedUserId = Convert.ToInt32(HttpContext.User.FindFirstValue("loggedUserId"));

            Expression<Func<Project, bool>> filter =
            p => 
                (
                    p.OwnerId == loggedUserId || 
                    p.Members.Any(m => m.Id == loggedUserId)
                ) && // only my projects will be visible
                (string.IsNullOrEmpty(model.Filter.Title) || p.Title.Contains(model.Filter.Title)) &&
                (string.IsNullOrEmpty(model.Filter.Description) || p.Description.Contains(model.Filter.Description)) &&
                (model.Filter.OwnerId == null || p.OwnerId == model.Filter.OwnerId);

           return Ok(service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "Project not found");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");   
            ProjectServices service = new ProjectServices();
            var projectResult = service.GetById(id);
            if (projectResult is null)
            {
                return NotFound(ModelState);
            }
            return Ok(projectResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProjectRequest model)
        {
            ProjectServices service = new ProjectServices();

            string loggedUserId = this.HttpContext.User.FindFirst("loggedUserId")?.Value;

        
            var item = new Project
            {
                OwnerId = Convert.ToInt32(loggedUserId),
                Title = model.Title,
                Description = model.Description,
            };
            
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            service.Save(item);
            
            // return Ok(model);
            return Ok(model);
        }

        [HttpDelete] // for single parameter
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "Project not found");
            ProjectServices service = new ProjectServices();
            var forDeleteResult = service.GetById(id);
            if (forDeleteResult is null)
                // throw new System.Exception("User not found");
                return NotFound(ModelState);

            service.Delete(forDeleteResult);

            return Ok(forDeleteResult);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] ProjectRequest model)
        {
            ProjectServices service = new ProjectServices();
            ModelState.AddModelError("Global", "Project not found");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var forUpdateResult = service.GetById(id);
            if (forUpdateResult is null)
                //throw new System.Exception("User not found");
                return NotFound(ModelState);

            Project forUpdate = forUpdateResult;  
            forUpdate.Title = model.Title;
            forUpdate.Description = model.Description;
            
            service.Save(forUpdate);
            // return Ok(model);
            return Ok(model);
        }

        // /api/projects/2/addmember?userId=5
        [Route("{projectId}/addmember")]
        [HttpGet]
        public IActionResult AddMember([FromRoute]int projectId, int userId)
        {
            return Ok("ProjectsController is working");
        }
    }
}
