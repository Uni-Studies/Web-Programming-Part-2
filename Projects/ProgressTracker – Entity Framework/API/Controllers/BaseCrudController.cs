using System;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Tasks;
using API.Infrastructure.ResponseDTOs.Shared;
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
    where EService : BaseService<E>, new()
    where ERequest : class, new()
    where EGetRequest : BaseGetRequest, new()
    where EGetResponse : BaseGetResponse<E>, new()
    {
        protected virtual void PopulateEntity(E item, ERequest model, out string error)
        { error = null; }

        protected virtual Expression<Func<E, bool>> GetFilter(EGetRequest model)
        { return null; }

        protected virtual void PopulateGetResponse(EGetRequest request, EGetRequest response)
        {}

        
    }
}
