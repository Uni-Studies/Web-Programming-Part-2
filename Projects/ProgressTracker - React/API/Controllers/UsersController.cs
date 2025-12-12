using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Infrastructure.RequestDTOs.Projects;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Users;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Users;
using Common;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseCrudController<User, UserServices, UserRequest, UsersGetRequest, UsersGetResponse>
    {
        protected override void PopulateEntity(User item, UserRequest model, out string error)
        {
            error = null;

            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
        }

        protected override Expression<Func<User, bool>> GetFilter(UsersGetRequest model)
        {
            return
                u =>
                    (string.IsNullOrEmpty(model.Filter.Username) || u.Username.Contains(model.Filter.Username)) &&
                    (string.IsNullOrEmpty(model.Filter.FirstName) || u.FirstName.Contains(model.Filter.FirstName)) &&
                    (string.IsNullOrEmpty(model.Filter.LastName) || u.LastName.Contains(model.Filter.LastName));
        }

        protected override void PopulateGetResponse(UsersGetRequest request, UsersGetResponse response)
        {
            response.Filter = request.Filter;
        }
    }
}
