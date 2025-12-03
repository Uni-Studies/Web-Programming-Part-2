using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.WorkLogs;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkLogsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromBody] WorkLogGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                    ? 10
                                    : model.Pager.PageSize;

            model.OrderBy ??= "Id";
            model.OrderBy = typeof(WorkLog).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : "Id";

            model.Filter ??= new WorkLogGetFilterRequest();

            WorkLogServices service = new WorkLogServices();

            int loggedUserId =
                Convert.ToInt32(
                    this.User.FindFirstValue("loggedUserId")
                );

            Expression<Func<WorkLog, bool>> filter = w =>
                (
                    w.Task.Project.OwnerId == loggedUserId ||
                    w.Task.Project.Members.Any(u => u.Id == loggedUserId)
                ) &&
                (model.Filter.UserId == null || w.UserId == model.Filter.UserId) &&
                (model.Filter.TaskId == null || w.TaskId == model.Filter.TaskId) &&
                (model.Filter.FromDate == null || w.LogDate >= model.Filter.ToDate.Value.ToDateTime(TimeOnly.MaxValue) &&
                (model.Filter.ToDate == null || w.LogDate <= model.Filter.ToDate.Value.ToDateTime(TimeOnly.MaxValue)));

            return Ok(
                service.GetAll(
                    filter,
                    model.OrderBy,
                    model.SortAsc,
                    model.Pager.Page,
                    model.Pager.PageSize
                )
            );
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            WorkLogServices service = new WorkLogServices();
            return Ok(service.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] WorkLogRequest model)
        {
            WorkLogServices service = new WorkLogServices();

            var item = new WorkLog()
            {
                UserId = model.UserId,
                TaskId = model.TaskId,
                LogDate = model.LogDate == default ? DateTime.UtcNow : model.LogDate
            };

            service.Save(item);

            return Ok(item);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            WorkLogServices service = new WorkLogServices();
            WorkLog forDelete = service.GetById(id);
            if (forDelete == null)
                throw new Exception("WorkLog not found");

            service.Delete(forDelete);

            return Ok(forDelete);
        }
    }
}
