using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Shared;
using Castle.Components.DictionaryAdapter;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseCRUDController<E, EService, ERequest, EGetRequest, EGetResponse> : ControllerBase
    where E : BaseEntity, new()
    where EService : BaseServices<E>, new()
    where ERequest : class, new()
    where EGetRequest : BaseGetRequest, new()
    where EGetResponse : BaseGetResponse<E>, new()
    {
        protected virtual void PopulateEntity(E item, ERequest model, out string error)
        {
            error = null;
        }

        protected virtual Expression<Func<E, bool>> GetFilter(EGetRequest model)
        {
            return null;
        }
        protected virtual void PopulateGetResponse(EGetRequest request, EGetResponse response)
        {
        }

        [HttpGet]
        public virtual IActionResult Get([FromQuery] EGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(E).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);

            EService service = new EService();

            Expression<Func<E, bool>> filter = GetFilter(model);

            var response = new EGetResponse();    

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

            return Ok(ServiceResult<EGetResponse>.Success(response));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            EService service = new EService();
            var item = service.GetById(id);
            return Ok(ServiceResult<E>.Success(item));
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody] ERequest model)
        {
            EService service = new EService();
            E item = new E();
            PopulateEntity(item, model, out string error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(ServiceResult<ERequest>.Failure(model,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>(){ error }
                        }
                    }));
            }

            service.Save(item);
            return Ok(ServiceResult<E>.Success(item));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] ERequest model)
        {
            EService service = new EService();
            E forUpdate = service.GetById(id);
            if(forUpdate == null)
                throw new Exception($"{typeof(E).Name} not found");

            PopulateEntity(forUpdate, model, out string error);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(ServiceResult<ERequest>.Failure(model,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>(){ error }
                        }
                    }));
            }
            service.Save(forUpdate);

            return Ok(ServiceResult<E>.Success(forUpdate));
        }

        [HttpDelete]
        [Route("{id}")]

        public IActionResult Delete([FromRoute] int id)
        {
            EService service = new EService();
            E forDelete = service.GetById(id);
            if(forDelete == null)
                throw new Exception($"{typeof(E).Name} not found");

            service.Delete(forDelete);
            
            return Ok(ServiceResult<E>.Success(forDelete));
        }
    }
}
