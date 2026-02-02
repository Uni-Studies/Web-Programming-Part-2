using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Education;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Education;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Services;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = Common.Error;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : BaseCRUDController<Education, EducationServices, EducationRequest, EducationsGetRequest, EducationsGetResponse>
    {
        protected override void PopulateEntity(Education item, EducationRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.CompletionStatus = model.CompletionStatus;
            item.Type = model.Type;
            item.Specialty = model.Specialty;
            item.School = model.School;
            item.Location = model.Location;
        }

        protected override Expression<Func<Education, bool>> GetFilter(EducationsGetRequest model)
        {
            model.Filter ??= new EducationsGetFilterRequest();
            return
                p =>
                    (string.IsNullOrEmpty(model.Filter.Type) || p.Type.Contains(model.Filter.Type)) &&
                    (string.IsNullOrEmpty(model.Filter.Specialty) || p.Specialty.Contains(model.Filter.Specialty)) &&
                    (string.IsNullOrEmpty(model.Filter.School) || p.School.Contains(model.Filter.School)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || p.Location.Contains(model.Filter.Location));
        }

        protected override Expression<Func<Education, bool>> GetPersonalFilter(EducationsGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new EducationsGetFilterRequest();
            return
                e =>
                    (e.UserId == loggedUserId) && 
                    (string.IsNullOrEmpty(model.Filter.Type) || e.Type.Contains(model.Filter.Type)) &&
                    (string.IsNullOrEmpty(model.Filter.Specialty) || e.Specialty.Contains(model.Filter.Specialty)) &&
                    (string.IsNullOrEmpty(model.Filter.School) || e.School.Contains(model.Filter.School)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || e.Location.Contains(model.Filter.Location));
        }

        protected override void PopulateGetResponse(EducationsGetRequest request, EducationsGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [Authorize]
        [HttpGet("getPersonalOne/{educationId}")]
        public IActionResult GetPersonalOne([FromRoute] int educationId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            EducationServices service = new EducationServices();
            try
            {
                var item = service.GetEducationByUserId(loggedUserId, educationId);
                return Ok(ServiceResult<Education>.Success(item));
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
        [HttpGet("getPersonalEducations")]
        public virtual IActionResult GetPersonalEducations([FromQuery] EducationsGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Education).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            EducationServices service = new EducationServices();

            Expression<Func<Education, bool>> filter = GetPersonalFilter(model);

            var response = new EducationsGetResponse();    

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

            return Ok(ServiceResult<EducationsGetResponse>.Success(response));
        }
    }
}
