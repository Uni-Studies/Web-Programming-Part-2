using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Skill;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Skill;
using API.Infrastructure.ResponseDTOs.User;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        protected virtual void PopulateGetResponse(UsersGetRequest request, UsersGetResponse response)
        {
            request.Filter = response.Filter;
        }

        [HttpGet("viewSkillsImportance")]
        public IActionResult ViewSkillsImportance()
        {
            SkillServices skillServices = new SkillServices();
            var skillsImportance = skillServices.GetSkillsByImportance();

            var response = new SkillsGetResponse
            {
                SkillImportance = skillsImportance
            };

            return Ok(ServiceResult<SkillsGetResponse>.Success(response));
        }

        protected Expression<Func<User, bool>> GetFilter(UsersGetRequest model)
        {
            //int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            model.Filter ??= new UsersGetFilterRequest();

            
            model.Filter ??= new UsersGetFilterRequest();

            return u =>
                (!model.Filter.UserId.HasValue || u.Id == model.Filter.UserId.Value) &&
                (string.IsNullOrEmpty(model.Filter.FirstName) || u.FirstName.Contains(model.Filter.FirstName)) &&
                (string.IsNullOrEmpty(model.Filter.LastName) || u.LastName.Contains(model.Filter.LastName));
            }

        [HttpGet("getUsersBySkill")]
        public IActionResult GetUsersBySkill([FromBody] SkillRequest skill, [FromQuery] UsersGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;

            model.OrderBy ??= nameof(BaseEntity.Id);
            
            Expression<Func<User, bool>> filter = GetFilter(model);
            
            SkillServices skillServices = new SkillServices();

            var users = new List<User>();
            try
            {
                users = skillServices.GetUsersBySkill(skill.Name, 
                                                        filter,
                                                        model.OrderBy,
                                                        model.SortAscending,
                                                        model.Pager.Page,
                                                        model.Pager.PageSize);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return NotFound(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
            

            var response = new UsersGetResponse();    
            
            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;
            response.OrderBy = model.OrderBy;
            response.SortAscending = model.SortAscending;

            PopulateGetResponse(model, response);

            response.Pager.Count = users.Count;
            response.Items = users;

            return Ok(ServiceResult<UsersGetResponse>.Success(response));
        }
    }
}
