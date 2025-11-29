using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.WorkLogs;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromBody] WorkLogGetRequest model)
        {
            model ??= new WorkLogGetRequest();
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;

            model.OrderBy ??= "Id";
            model.OrderBy = typeof(WorkLog).GetProperty(model.OrderBy) != null ? model.OrderBy : "Id";
            model.Filter ??= new WorkLogGetFilterRequest();

            var service = new WorkLogServices();
            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));

          Expression<Func<WorkLog, bool>> filter =
            w => 
                (
                    w.Task.Project.OwnerId == loggedUserId || 
                    w.Task.Project.Members.Any(t => t.Id == loggedUserId)
                ) && // only my projects will be visible
                (model.Filter.UserId == null || w.UserId == model.Filter.UserId) &&
                (model.Filter.TaskId == null || w.TaskId == model.Filter.TaskId) &&
                (model.Filter.CreatedAtFrom == null || t.CreatedAt >= model.Filter.CreatedAtFrom) &&
                (model.Filter.CreatedAtTo == null || t.CreatedAt <= model.Filter.CreatedAtTo) &&
                (model.Filter.DueDateFrom == null || t.DueDate >= model.Filter.DueDateFrom) &&
                (model.Filter.DueDateTo == null || t.DueDate <= model.Filter.DueDateTo);
              return Ok(service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize)); 
        
            var result = service.GetAll(filter, model.OrderBy, model.SortAsc, model.Pager.Page, model.Pager.PageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var service = new WorkLogServices();
            var workLog = service.GetById(id);
            if (workLog is null)
            {
                ModelState.AddModelError("Global", "Work log not found");
                return NotFound(ModelState);
            }

            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));
            bool hasAccess = workLog.UserId == loggedUserId ||
                             (workLog.Task != null && (
                                 workLog.Task.Project.OwnerId == loggedUserId ||
                                 workLog.Task.Project.Members.Any(m => m.Id == loggedUserId)
                             ));

            if (!hasAccess)
            {
                ModelState.AddModelError("Global", "You do not have access to this work log.");
                return Forbid();
            }

            return Ok(workLog);
        }

        [HttpPost]
        public IActionResult Post([FromBody] WorkLogRequest model)
        {
            var service = new WorkLogServices();
            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));

            var item = new WorkLog
            {
                UserId = loggedUserId,
                TaskId = model.TaskId,
                WorkDuration = model.WorkDuration,
                LogDate = model.LogDate
            };

            service.Save(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] WorkLogRequest model)
        {
            var service = new WorkLogServices();
            var forUpdate = service.GetById(id);
            if (forUpdate == null)
            {
                ModelState.AddModelError("Global", "Work log not found");
                return NotFound(ModelState);
            }

            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));
            if (forUpdate.UserId != loggedUserId)
            {
                ModelState.AddModelError("Global", "You do not have permission to update this work log.");
                return Forbid();
            }

            forUpdate.TaskId = model.TaskId;
            forUpdate.WorkDuration = model.WorkDuration;
            forUpdate.LogDate = model.LogDate;

            service.Save(forUpdate);
            return Ok(forUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var service = new WorkLogServices();
            var forDeleteResult = service.GetById(id);
            if (forDeleteResult is null)
            {
                ModelState.AddModelError("Global", "Work log not found");
                return NotFound(ModelState);
            }

            int loggedUserId = Convert.ToInt32(this.User.FindFirstValue("loggedUserId"));
            if (forDeleteResult.UserId != loggedUserId)
            {
                ModelState.AddModelError("Global", "You do not have permission to delete this work log.");
                return Forbid();
            }

            service.Delete(forDeleteResult);
            return Ok(forDeleteResult);
        }
    }
}

