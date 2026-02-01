using System;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Project;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Project;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseCRUDController<Project, ProjectServices, ProjectRequest, ProjectsGetRequest, ProjectsGetResponse>
    {
        protected override void PopulateEntity(Project item, ProjectRequest model, out string error)
        {
            error = null; 
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.Type = model.Type;
            item.Title = model.Title;
            item.Topic = model.Topic;
            item.Mentor = model.Mentor;
            item.PagesCount = model.PagesCount;
            item.Language = model.Language;
            item.Description = model.Description;
            item.Link = model.Link;
            item.CompletionStatus = model.CompletionStatus;
        }

        protected override Expression<Func<Project, bool>> GetFilter(ProjectsGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new ProjectsGetFilterRequest();    
            return 
                p => 
                    (string.IsNullOrEmpty(model.Filter.Type) || p.Type.Contains(model.Filter.Type)) &&
                    (string.IsNullOrEmpty(model.Filter.Title) || p.Title.Contains(model.Filter.Title)) &&
                    (string.IsNullOrEmpty(model.Filter.Topic) || p.Topic.Contains(model.Filter.Topic)) &&
                    (string.IsNullOrEmpty(model.Filter.Mentor) || p.Mentor.Contains(model.Filter.Mentor)) &&
                
                    (string.IsNullOrEmpty(model.Filter.Language) || p.Language.Contains(model.Filter.Language)) &&
                    (p.CompletionStatus == model.Filter.CompletionStatus);
        }

        protected override void PopulateGetResponse(ProjectsGetRequest request, ProjectsGetResponse response)
        {
            response.Filter = request.Filter;
        }
    }
}
