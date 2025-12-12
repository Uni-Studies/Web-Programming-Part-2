using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Tasks;
using API.Infrastructure.ResponseDTOs.Tasks;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseCrudController<Task, TaskServices, TaskRequest, TasksGetRequest, TasksGetResponse>
    {
        protected override Expression<Func<Task, bool>> GetFilter(TasksGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(
                    this.User.FindFirst("loggedUserId").Value
                );
            
            return
                t =>
                    (
                        t.Project.OwnerId == loggedUserId ||
                        t.Project.Members.Any(u => u.Id == loggedUserId)
                    ) &&
                    (string.IsNullOrEmpty(model.Filter.Title) || t.Title.Contains(model.Filter.Title)) &&
                    (string.IsNullOrEmpty(model.Filter.Description) || t.Description.Contains(model.Filter.Description)) &&
                    (model.Filter.AssigneeId == null || t.AssigneeId == model.Filter.AssigneeId) &&
                    (model.Filter.ProjectId == null || t.ProjectId == model.Filter.ProjectId) &&
                    (model.Filter.CreatedAtFrom == null || t.CreatedAt >= model.Filter.CreatedAtFrom) &&
                    (model.Filter.CreatedAtTo == null || t.CreatedAt <= model.Filter.CreatedAtTo) &&
                    (model.Filter.DueDateFrom == null || t.DueDate >= model.Filter.DueDateFrom) &&
                    (model.Filter.DueDateTo == null || t.DueDate <= model.Filter.DueDateTo);
        }

        protected override void PopulateGetResponse(TasksGetRequest request, TasksGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override void PopulateEntity(Task item, TaskRequest model, out string error)
        {
            error = null;

            int loggedUserId = Convert.ToInt32(
                this.User.FindFirst("loggedUserId").Value
            );

            item.OwnerId = loggedUserId;
            item.AssigneeId = loggedUserId;
            item.ProjectId = model.ProjectId;
            item.Title = model.Title;
            item.Description = model.Description;
            item.CreatedAt = DateTime.UtcNow;
            item.DueDate = model.DueDate;
        }
    }
}
