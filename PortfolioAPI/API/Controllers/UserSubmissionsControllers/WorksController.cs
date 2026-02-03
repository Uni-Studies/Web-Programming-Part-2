using System;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Work;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Work;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : BaseCRUDController<Work, WorkServices, WorkRequest, WorkGetRequest, WorkGetResponse>
    {
        protected override void PopulateEntity(Work item, WorkRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.Sphere = model.Sphere;
            item.Occupation = model.Occupation;
            item.Location = model.Location;
            item.Salary = model.Salary;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.Company = model.Company;
        }

        protected override Expression<Func<Work, bool>> GetFilter(WorkGetRequest model)
        {
            model.Filter ??= new WorkGetFilterRequest();    
            return 
                p => 
                    (string.IsNullOrEmpty(model.Filter.Sphere) || p.Sphere.Contains(model.Filter.Sphere)) &&
                    (string.IsNullOrEmpty(model.Filter.Occupation) || p.Occupation.Contains(model.Filter.Occupation)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || p.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.Company) || p.Company.Contains(model.Filter.Company)) &&
                
                    (!model.Filter.SalaryFrom.HasValue || p.Salary >= model.Filter.SalaryFrom.Value) &&
                    (!model.Filter.SalaryTo.HasValue || p.Salary <= model.Filter.SalaryTo.Value);
        }

        protected override Expression<Func<Work, bool>> GetPersonalFilter(WorkGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new WorkGetFilterRequest();
            return
                w =>
                    (w.UserId == loggedUserId) &&
                    (string.IsNullOrEmpty(model.Filter.Sphere) || w.Sphere.Contains(model.Filter.Sphere)) &&
                    (string.IsNullOrEmpty(model.Filter.Occupation) || w.Occupation.Contains(model.Filter.Occupation)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || w.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.Company) || w.Company.Contains(model.Filter.Company)) &&
                    (!model.Filter.SalaryFrom.HasValue || w.Salary >= model.Filter.SalaryFrom.Value) &&
                    (!model.Filter.SalaryTo.HasValue || w.Salary <= model.Filter.SalaryTo.Value);
        }

        protected override void PopulateGetResponse(WorkGetRequest request, WorkGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [Authorize]
        [HttpGet("getPersonalOne/{workId}")]
        public IActionResult GetPersonalOne([FromRoute] int workId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            WorkServices service = new WorkServices();
            try
            {
                var item = service.GetWorkByUserId(loggedUserId, workId);
                return Ok(ServiceResult<Work>.Success(item));
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
        [HttpGet("getPersonalWorks")]
        public virtual IActionResult GetPersonalJobs([FromQuery] WorkGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Work).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            WorkServices service = new WorkServices();

            Expression<Func<Work, bool>> filter = GetPersonalFilter(model);

            var response = new WorkGetResponse();

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

            return Ok(ServiceResult<WorkGetResponse>.Success(response));
        }
    }
}
