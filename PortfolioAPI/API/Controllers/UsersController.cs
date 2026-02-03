using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.SocialNetwork;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.User;
using API.Services;
using Common;
using Common.Entities;
using Common.Entities.DTOs;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseCRUDController<User, UserServices, UserRequest, UsersGetRequest, UsersGetResponse>
    {
        protected override void PopulateEntity(User item, UserRequest model)
        {
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
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
            //int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            model.Filter ??= new UsersGetFilterRequest();

            
            model.Filter ??= new UsersGetFilterRequest();

            return u =>
                (!model.Filter.UserId.HasValue || u.Id == model.Filter.UserId.Value) &&
                (string.IsNullOrEmpty(model.Filter.FirstName) || u.FirstName.Contains(model.Filter.FirstName)) &&
                (string.IsNullOrEmpty(model.Filter.LastName) || u.LastName.Contains(model.Filter.LastName));
            }

        protected override void PopulateGetResponse(UsersGetRequest request, UsersGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [Authorize]
        [HttpPost]
        public override IActionResult Post([FromBody] UserRequest model)
        {
            return Unauthorized("Users cannot create new users. Use /auth/register instead.");
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] UserRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
                
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var forUpdate = userServices.GetById(loggedUserId);

            if (forUpdate == null) //admin validation
            {
                return NotFound(
                    ServiceResult<UserRequest>.Failure(
                        model,
                        new List<Error>
                        {
                            new Error
                            {
                                Key = "Global",
                                Messages = new List<string>
                                {
                                    "User not found."
                                }
                            }
                        }
                    )
                );
              
            }
            PopulateEntity(forUpdate, model);
           
            userServices.Save(forUpdate);

            return Ok(ServiceResult<User>.Success(forUpdate));
        }

        [HttpGet("getBio/{userId}")]
        public IActionResult GetBio([FromRoute]int userId)
        {
            UserServices userServices = new UserServices();
            var user = userServices.GetById(userId);

            if(user is null)
                 return NotFound(
                    ServiceResult<UserRequest>.Failure(
                        null,
                        new List<Error>
                        {
                            new Error
                            {
                                Key = "Global",
                                Messages = new List<string>
                                {
                                    "User not found."
                                }
                            }
                        }
                    )
                );
            FullUser fullUser = userServices.GetFullUser(userId);
            return Ok(ServiceResult<FullUser                                                                                                                                                                                                                                                                                                                                                                                                                             >.Success(fullUser));
        }

        [HttpGet("getUserFullInfo/{userId}")]
        public IActionResult GetUserFullInfo([FromRoute]int userId)
        {
            UserServices userServices = new UserServices();
            var user = userServices.GetById(userId);

            if(user is null)
                 return NotFound(
                    ServiceResult<UserRequest>.Failure(
                        null,
                        new List<Error>
                        {
                            new Error
                            {
                                Key = "Global",
                                Messages = new List<string>
                                {
                                    "User not found."
                                }
                            }
                        }
                    )
                );
            FullUser fullUser = userServices.GetFullUser(userId);
            
            var response = new FullUserExtension
            {
                Posts = user.Posts,
                SavedPosts = user.SavedPosts,
                SocialNetworks = user.SocialNetworks,
                Projects = user.Projects,
                Educations = user.Educations,
                Jobs = user.Jobs,
                Courses = user.Courses,
                Events = user.Events,
                UserSkills = user.UserSkills
            };

            return Ok(ServiceResult<FullUserExtension>.Success(response));
        }

        [Authorize]
        [HttpPost("addSocialNetwork")]
        public IActionResult AddSocialNetwork([FromBody] SocialNetworkRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
                
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
          
            SocialNetwork socialNetwork = new() { Type = model.Type, Account = model.Account, Link = model.Link };
           
            socialNetwork.UserId = loggedUserId;
            /* socialNetwork.User = user; */

            SocialNetworkServices socialNetworkServices = new SocialNetworkServices();
            socialNetworkServices.Save(socialNetwork); 

            return Ok(ServiceResult<SocialNetwork>.Success(socialNetwork));
        }

        [Authorize]
        [HttpPost("removeSocialNetwork/{id}")]
        public IActionResult RemoveSocialNetwork([FromRoute]int id)
        {             
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            SocialNetworkServices socialNetworkServices = new SocialNetworkServices();
            var socialNetwork = socialNetworkServices.GetById(id);

            if (socialNetwork == null)
                return NotFound($"Social network with id {id} not found.");

            if (socialNetwork.UserId != loggedUserId)
                return Forbid("You cannot remove another user's social network.");

            socialNetworkServices.Delete(socialNetwork);

            return Ok(ServiceResult<SocialNetwork>.Success(socialNetwork));
        }
    }
}
