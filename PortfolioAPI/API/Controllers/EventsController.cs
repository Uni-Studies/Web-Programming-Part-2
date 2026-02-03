using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using API.Infrastructure.RequestDTOs.Event;
using API.Infrastructure.ResponseDTOs.Event;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Error = Common.Error;
using API.Services;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : BaseCRUDController<Event, EventServices, EventRequest, EventGetRequest, EventsGetResponse>
    {
        protected override void PopulateEntity(Event item, EventRequest model)
        {
            item.Title = model.Title;
            item.Description = model.Description;
            item.Location = model.Location;
            item.Type = model.Type;
            item.Capacity = model.Capacity;
            item.Price = model.Price;
            item.DeadlineDate = model.DeadlineDate;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.StartTime = model.StartTime;
            item.EndTime = model.EndTime;
        }

        protected override Expression<Func<Event, bool>> GetFilter(EventGetRequest model)
        {
            
            model.Filter ??= new EventsGetFilterRequest();

            return 
                e =>
                    (string.IsNullOrEmpty(model.Filter.Title) || e.Title.Contains(model.Filter.Title)) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || e.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.Type) || e.Type.Contains(model.Filter.Type)) &&
                    (model.Filter.Price == 0 || e.Price == model.Filter.Price) &&
                    (model.Filter.Capacity == 0 || e.Capacity == model.Filter.Capacity) &&
                    (model.Filter.StartDate == default || e.StartDate >= model.Filter.StartDate) &&
                    (model.Filter.EndDate == default || e.EndDate <= model.Filter.EndDate) &&
                    (model.Filter.StartTime == default || e.StartTime >= model.Filter.StartTime) &&
                    (model.Filter.EndTime == default || e.EndTime <= model.Filter.EndTime);
        }

      
        protected override void PopulateGetResponse(EventGetRequest request, EventsGetResponse response)
        {
            response.Filter = request.Filter;
        }


        [Authorize]
        [HttpGet("getPersonalEvents")]
        public virtual IActionResult GetPersonalEvents([FromQuery] EventGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Event).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            EventServices service = new EventServices();

            Expression<Func<Event, bool>> filter = GetFilter(model);

            var response = new EventsGetResponse();

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

            return Ok(ServiceResult<EventsGetResponse>.Success(response));
        }
    }
}
