using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Tasks;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromBody] TaskGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0 
                                            ? 1 
                                            : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                            ? 10 
                                            : model.Pager.PageSize;
                                        
            model.OrderBy ??= "Id";
            model.OrderBy = typeof(Task).GetProperty(model.OrderBy) != null 
                                            ? model.OrderBy 
                                            : "id";
            model.Filter ??= new TaskGetFilterRequest();
            
            TaskServices service = new TaskServices();           

            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));

            Expression<Func<Task, bool>> filter =
            t => 
                (
                    t.Project.OwnerId == loggedUserId || 
                    t.Project.Members.Any(t => t.Id == loggedUserId)
                ) && // only my projects will be visible
                (string.IsNullOrEmpty(model.Filter.Title) || t.Title.Contains(model.Filter.Title)) &&
                (string.IsNullOrEmpty(model.Filter.Description) || t.Description.Contains(model.Filter.Description)) &&
                (model.Filter.AssigneeId == null || t.AssigneeId == model.Filter.AssigneeId) &&
                (model.Filter.ProjectId == null || t.ProjectId == model.Filter.ProjectId) &&
                (model.Filter.CreatedAtFrom == null || t.CreatedAt >= model.Filter.CreatedAtFrom) &&
                (model.Filter.CreatedAtTo == null || t.CreatedAt <= model.Filter.CreatedAtTo) &&
                (model.Filter.DueDateFrom == null || t.DueDate >= model.Filter.DueDateFrom) &&
                (model.Filter.DueDateTo == null || t.DueDate <= model.Filter.DueDateTo);
              return Ok(service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize)); 
        }

        public IActionResult Get([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "Task not found");
            //string loggedUserId = this.User.FindFirstValue("loggedUserId");   
            TaskServices service = new TaskServices();
            var taskResult = service.GetById(id);
            if (taskResult is null)
            {
                return NotFound(ModelState);
            }

            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));
            if (taskResult.Project.OwnerId != loggedUserId &&
                !taskResult.Project.Members.Any(m => m.Id == loggedUserId))
            {
                ModelState.AddModelError("Global", "You do not have access to this task.");
                return Forbid();
            }

            return Ok(taskResult);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskRequest model)
        {
            TaskServices service = new TaskServices();

            string loggedUserId = this.User.FindFirst("loggedUserId")?.Value;

        
            var item = new Task
            {
                OwnerId = Convert.ToInt32(loggedUserId),
                AssigneeId = model.AssigneeId,
                ProjectId = model.ProjectId,
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = model.DueDate
            };

            service.Save(item);
            return Ok(model);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] TaskRequest model)
        {
            TaskServices service = new TaskServices();
            var forUpdate = service.GetById(id);
            if (forUpdate == null)
            {
                ModelState.AddModelError("Global", "Task not found");
                return NotFound(ModelState);
            }

            forUpdate.AssigneeId = model.AssigneeId;
            //forUpdate.ProjectId = model.ProjectId; - dangerous
            forUpdate.Title = model.Title;
            forUpdate.Description = model.Description;
            forUpdate.DueDate = model.DueDate;

            service.Save(forUpdate);
            return Ok(model);
        }

        [HttpDelete] // for single parameter
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            ModelState.AddModelError("Global", "Task not found");
            TaskServices service = new TaskServices();
            var forDeleteResult = service.GetById(id);
            if (forDeleteResult is null)
                // throw new System.Exception("User not found");
                return NotFound(ModelState);

            service.Delete(forDeleteResult);

            return Ok(forDeleteResult);
        }
    }
}
