using System;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.User;
using Common;
using Common.Entities;
using Common.Entities.DTOs;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseCRUDController<User, UserServices, UserRequest, UsersGetRequest, UsersGetResponse>
    {
         protected override void PopulateEntity(User item, UserRequest model, out string error)
        {
            error = null; 
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            if(item.Id != loggedUserId)
            {
                    error = "Unauthorized";
                    return;
            }

            item.Sex = model.Sex;
            item.BirthDate = model.BirthDate;
            item.BirthCity = model.BirthCity;
            item.Address = model.Address;
            item.Country = model.Country;
            item.Nationality = model.Nationality;
            item.Details = model.Details;
            item.ProfilePicture = model.ProfilePicture;
        }

        protected override Expression<Func<User, bool>> GetFilter(UsersGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            UserServices userServices = new UserServices();
            FullUser user = userServices.GetFullUser(loggedUserId);

            model.Filter ??= new UsersGetFilterRequest();

            return u =>
                (u.Id == loggedUserId) &&
                (model.Filter.UserId == null || u.Id == model.Filter.UserId) &&
                (string.IsNullOrEmpty(model.Filter.Username) || user.Username.Contains(model.Filter.Username)) &&
                (string.IsNullOrEmpty(model.Filter.Email) || user.Email.Contains(model.Filter.Email)) &&
                (string.IsNullOrEmpty(model.Filter.FirstName) || u.FirstName.Contains(model.Filter.FirstName)) &&
                (string.IsNullOrEmpty(model.Filter.LastName) || u.LastName.Contains(model.Filter.LastName));
        }

        protected override void PopulateGetResponse(UsersGetRequest request, UsersGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [HttpPost]
        public override IActionResult Post([FromBody] UserRequest model)
        {
            return Unauthorized("Users cannot create new users. Use /auth/register instead.");
        }


        [HttpPost("addSocialNetwork")]
        public IActionResult AddSocialNetwork([FromBody] SocialNetwork socialNetwork)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            UserServices userServices = new UserServices();
            FullUser user = userServices.GetFullUser(loggedUserId);

            socialNetwork.User = user.User;

            SocialNetworkServices socialNetworkServices = new SocialNetworkServices();
            socialNetworkServices.Save(socialNetwork);

            return Ok(ServiceResult<SocialNetwork>.Success(socialNetwork));
        }
    }
}
