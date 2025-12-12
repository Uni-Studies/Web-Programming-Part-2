using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Projects;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Projects;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : BaseCrudController<Project, ProjectServices, ProjectRequest, ProjectsGetRequest, ProjectsGetResponse>
    {
        protected override void PopulateEntity(Project item, ProjectRequest model, out string error)
        {
            error = null;

            int loggedUserId = Convert.ToInt32(
                this.User.FindFirst("loggedUserId").Value
            );

            item.OwnerId = loggedUserId;
            item.Title = model.Title;
            item.Description = model.Description;
        }

        protected override Expression<Func<Project, bool>> GetFilter(ProjectsGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(
                this.User.FindFirst("loggedUserId").Value
            );
            
            return
                p =>
                    (
                        p.OwnerId == loggedUserId ||
                        p.Members.Any(u => u.Id == loggedUserId)
                    ) &&
                    (string.IsNullOrEmpty(model.Filter.Title) || p.Title.Contains(model.Filter.Title)) &&
                    (string.IsNullOrEmpty(model.Filter.Description) || p.Description.Contains(model.Filter.Description)) &&
                    (model.Filter.OwnerId == null || p.OwnerId == model.Filter.OwnerId);
        }

        protected override void PopulateGetResponse(ProjectsGetRequest request, ProjectsGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [Route("{projectId}/addmember/{userId}")]
        [HttpGet]
        public IActionResult AddMember([FromRoute]int projectId, [FromRoute]int userId)
        {
            ProjectServices projectService = new ProjectServices();
            UserServices userService = new UserServices();

            var project = projectService.GetById(projectId);
            if (project == null)    
                return BadRequest(ServiceResult<Project>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Project not found!" }
                        }
                    }));

            var user = userService.GetById(userId);
            if (user == null)    
                return BadRequest(ServiceResult<Project>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "User not found!" }
                        }
                    }));

            project.Members.Add(user);
            projectService.Save(project);

            return Ok(ServiceResult<Project>.Success(project));
        }

        [Route("{projectId}/revokemember/{userId}")]
        [HttpGet]
        public IActionResult RevokeMember([FromRoute]int projectId, [FromRoute]int userId)
        {
            ProjectServices projectService = new ProjectServices();
            UserServices userService = new UserServices();

            var project = projectService.GetById(projectId);
            if (project == null)    
                return BadRequest(ServiceResult<Project>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Project not found!" }
                        }
                    }));

            var user = userService.GetById(userId);
            if (user == null)    
                return BadRequest(ServiceResult<Project>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "User not found!" }
                        }
                    }));

            var toRemove = project.Members.FirstOrDefault(u => u.Id == userId);
            if (toRemove != null)
            {
                project.Members.Remove(toRemove);
                projectService.Save(project);
            }

            return Ok(ServiceResult<Project>.Success(project));
        }
    }
}
