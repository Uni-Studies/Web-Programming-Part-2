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
            SkillServices skillServices = new SkillServices();

            var users = new List<User>();
            try
            {
                users = skillServices.GetUsersBySkill(skill.Name);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return NotFound(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
            Expression<Func<User, bool>> filter = GetFilter(model);

            var response = new UsersGetResponse();    
            response.Items = users;

            return Ok(ServiceResult<UsersGetResponse>.Success(response));
        }
    }
}
