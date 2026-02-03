using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Course;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Course;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Services;
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
    public class CoursesController : BaseCRUDController<Course, CourseServices, CourseRequest, CoursesGetRequest, CoursesGetResponse>
    {
        protected override void PopulateEntity(Course item, CourseRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.Name = model.Name;
            item.Price = model.Price;
            item.Tutor = model.Tutor;
            item.Description = model.Description;
            item.Place = model.Place;
            item.Language = model.Language;
            item.HasCertificate = model.HasCertificate;
        }

        protected override Expression<Func<Course, bool>> GetFilter(CoursesGetRequest model)
        {
            model.Filter ??= new CoursesGetFilterRequest();
            return
                p =>
                    (string.IsNullOrEmpty(model.Filter.Name) || p.Name.Contains(model.Filter.Name)) &&
                    (string.IsNullOrEmpty(model.Filter.Tutor) || p.Tutor.Contains(model.Filter.Tutor)) &&
                    (string.IsNullOrEmpty(model.Filter.Description) || p.Description.Contains(model.Filter.Description)) &&
                    (string.IsNullOrEmpty(model.Filter.Place) || p.Place.Contains(model.Filter.Place)) &&
                    (string.IsNullOrEmpty(model.Filter.Language) || p.Language.Contains(model.Filter.Language)) &&
                    (model.Filter.Price == 0 || p.Price == model.Filter.Price);
        }

        protected override Expression<Func<Course, bool>> GetPersonalFilter(CoursesGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new CoursesGetFilterRequest();
            return
                c =>
                    (c.UserId == loggedUserId) &&
                    (string.IsNullOrEmpty(model.Filter.Name) || c.Name.Contains(model.Filter.Name)) &&
                    (string.IsNullOrEmpty(model.Filter.Tutor) || c.Tutor.Contains(model.Filter.Tutor)) &&
                    (string.IsNullOrEmpty(model.Filter.Description) || c.Description.Contains(model.Filter.Description)) &&
                    (string.IsNullOrEmpty(model.Filter.Place) || c.Place.Contains(model.Filter.Place)) &&
                    (string.IsNullOrEmpty(model.Filter.Language) || c.Language.Contains(model.Filter.Language)) &&
                    (model.Filter.Price == 0 || c.Price == model.Filter.Price);
        }

        protected override void PopulateGetResponse(CoursesGetRequest request, CoursesGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [Authorize]
        [HttpGet("getPersonalOne/{courseId}")]
        public IActionResult GetPersonalOne([FromRoute] int courseId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            CourseServices service = new CourseServices();
            try
            {
                var item = service.GetCourseByUserId(loggedUserId, courseId);
                return Ok(ServiceResult<Course>.Success(item));
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
        [HttpGet("getPersonalCourses")]
        public virtual IActionResult GetPersonalCourses([FromQuery] CoursesGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Course).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            CourseServices service = new CourseServices();

            Expression<Func<Course, bool>> filter = GetPersonalFilter(model);

            var response = new CoursesGetResponse();

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

            return Ok(ServiceResult<CoursesGetResponse>.Success(response));
        }
    }
}
