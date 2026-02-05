using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Project;
using API.Infrastructure.RequestDTOs.Skill;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Project;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Entities.ManyToManyEntities;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseCRUDController<Project, ProjectServices, ProjectRequest, ProjectsGetRequest, ProjectsGetResponse>
    {
        protected override void PopulateEntity(Project item, ProjectRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
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

        protected override Expression<Func<Project, bool>> GetPersonalFilter(ProjectsGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new ProjectsGetFilterRequest();
            return
                p =>
                    (p.UserId == loggedUserId) &&
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

        [Authorize]
        [HttpGet("getPersonalOne/{projectId}")]
        public IActionResult GetPersonalOne([FromRoute] int projectId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            ProjectServices service = new ProjectServices();
            try
            {
                var item = service.GetProjectByUserId(loggedUserId, projectId);
                return Ok(ServiceResult<Project>.Success(item));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return NotFound(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
        }

        [Authorize]
        [HttpGet("getPersonalProjects")]
        public virtual IActionResult GetPersonalProjects([FromQuery] ProjectsGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Project).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            ProjectServices service = new ProjectServices();

            Expression<Func<Project, bool>> filter = GetPersonalFilter(model);

            var response = new ProjectsGetResponse();

            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;
            response.OrderBy = model.OrderBy;
            response.SortAscending = model.SortAscending;

            PopulateGetResponse(model, response);

            response.Pager.Count = service.Count(filter);
            response.Items = service.GetAll(
                filter,
                model.OrderBy,
                model.SortAscending,
                model.Pager.Page,
                model.Pager.PageSize
            );

            return Ok(ServiceResult<ProjectsGetResponse>.Success(response));
        }

        [Authorize]
        [HttpPost("addSkill/{projectId}")]
        public IActionResult AddSkill([FromRoute] int projectId, [FromBody] SkillRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );

            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            ProjectServices projectServices = new ProjectServices();

            var project = projectServices.GetById(projectId);
            if (project is null)
                return NotFound(ServiceResult<Post>.Failure(null,
                   new List<Error>
                   {
                       new Error()
                       {
                           Key = "Global",
                           Messages = new List<string>() { "Project not found" }
                       }
                   }));

            if (project.UserId != loggedUserId)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "You cannot add skill to this project. You are not the owner." }
                        }
                    }));
            }

            model.Name = model.Name.Trim().ToUpper();
            SkillServices skillServices = new SkillServices();
            Skill item = new Skill() { Name = model.Name };

            if (!skillServices.SkillExists(model.Name))
                skillServices.Save(item);

            var savedSkill = skillServices.GetByName(model.Name);
            if (savedSkill is null)
                return NotFound(ServiceResult<Post>.Failure(null,
                   new List<Error>
                   {
                       new Error()
                       {
                           Key = "Global",
                           Messages = new List<string>() { "Skill not found" }
                       }
                   }));

            try
            {
                var userSkill = skillServices.AddSkillToSubmission(loggedUserId, savedSkill.Id, project, model.Importance);
                skillServices.MapUserSkillToUserAndSkill(user, savedSkill, userSkill);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }

            return Ok(ServiceResult<Skill>.Success(item));
        }

        [Authorize]
        [HttpDelete("removeSkill/{projectId}/{skillId}")]
        public IActionResult RemoveSkill([FromRoute] int projectId, [FromRoute] int skillId, [FromRoute] SkillsGetRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            ProjectServices projectServices = new ProjectServices();

            var project = projectServices.GetById(projectId);
            if (project is null)
                return NotFound(ServiceResult<Post>.Failure(null,
                   new List<Error>
                   {
                       new Error()
                       {
                           Key = "Global",
                           Messages = new List<string>() { "Project not found" }
                       }
                   }));

            if (project.UserId != loggedUserId)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "You cannot add skill to this project. You are not the owner." }
                        }
                    }));
            }

            UserSkillServices userSkillServices = new UserSkillServices();
            var userSkill = userSkillServices.GetById(loggedUserId, skillId, Common.Enums.SubmissionType.Project, projectId);
            userSkillServices.Delete(userSkill);

            return Ok(ServiceResult<UserSkill>.Success(userSkill));
        }
    }
}
