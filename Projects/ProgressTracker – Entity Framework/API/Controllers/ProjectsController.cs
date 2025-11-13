using API.Infrastructure.RequestDTOs.Projects;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
          [HttpGet]
        public IActionResult Get()
        {
            //ModelState.AddModelError("Global", "Empty users list");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");
            ProjectServices service = new ProjectServices();
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

        
            Project item = new Project
            {
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
            ModelState.AddModelError("Global", "User not found");
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
            ModelState.AddModelError("Global", "User not found");
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
    }
}
