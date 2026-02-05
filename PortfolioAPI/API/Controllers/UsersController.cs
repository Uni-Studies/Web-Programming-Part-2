using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Interest;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.SocialNetwork;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.Interest;
using API.Infrastructure.ResponseDTOs.Shared;
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
        }

        protected override Expression<Func<User, bool>> GetFilter(UsersGetRequest model)
        {
           
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

        [Authorize]
        [HttpGet("getUserPortfolio")]
        public IActionResult GetUserPortfolio()
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            FullUser fullUser = userServices.GetFullUser(loggedUserId);
            SkillServices skillServices = new SkillServices();
            var skills = skillServices.GetUserSkills(user);

            var response = new FullUserExtension
            {
                UserBio = fullUser,
                Posts = user.Posts,
                SavedPosts = user.SavedPosts,
                SocialNetworks = user.SocialNetworks,
                Projects = user.Projects,
                Educations = user.Educations,
                Jobs = user.Jobs,
                Courses = user.Courses,
                Interests = user.Interests,
                UserSkills = skills
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
          
            SocialNetwork socialNetwork = new() { Type = model.Type.Trim(), Account = model.Account.Trim(), Link = model.Link.Trim()};
           
            socialNetwork.UserId = loggedUserId;

            SocialNetworkServices socialNetworkServices = new SocialNetworkServices();
            if (socialNetworkServices.AccountExists(model.Account.Trim()))
            {
                return BadRequest(ServiceResult<SocialNetwork>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Account already exists" }
                        }
                    }));
            }
            socialNetworkServices.Save(socialNetwork); 

            return Ok(ServiceResult<SocialNetwork>.Success(socialNetwork));
        }

        [Authorize]
        [HttpDelete("removeSocialNetwork/{id}")]
        public IActionResult RemoveSocialNetwork([FromRoute]int id)
        {             
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            SocialNetworkServices socialNetworkServices = new SocialNetworkServices();
            var socialNetwork = socialNetworkServices.GetById(id);

            if (socialNetwork == null)
                return BadRequest(ServiceResult<SocialNetwork>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Social network not found" }
                        }
                    }));

            if (socialNetwork.UserId != loggedUserId)
                return Forbid("You cannot remove another user's social network.");

            socialNetworkServices.Delete(socialNetwork);

            return Ok(ServiceResult<SocialNetwork>.Success(socialNetwork));
        }

        #region Interests
        [Authorize]
        [HttpPost("addInterest")]
        public IActionResult AddInterest([FromBody] InterestRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
                
            model.Name = model.Name.Trim().ToLower();
            
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            if(user == null) // admin does not have a user; validation for admin
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "User not found" }
                        }
                    }));
            }

            InterestServices interestServices = new InterestServices();
            var interest = interestServices.GetByName(model.Name);

            if (interest is not null && interestServices.UserHasInterest(user, interest.Name))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "User has already added this interest" }
                        }
                    }));
            }

            if(interest is null)
            {
                interest = new Interest() { Name = model.Name };
                interestServices.Save(interest);
            }
            
            interestServices.AddInterestToUser(interest.Name, user);
            return Ok(ServiceResult<Interest>.Success(interest));
        }
        
        [Authorize]
        [HttpDelete("removeInterest")]
        public IActionResult RemoveInterest([FromBody] InterestRequest model)
        {
             if (!ModelState.IsValid)
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );

            model.Name = model.Name.Trim().ToLower();
            InterestServices interestService = new InterestServices();
            
            var interest = interestService.GetByName(model.Name);
            if(interest == null)
            {
                return BadRequest(ServiceResult<Interest>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Interest not found" }
                        }
                    }));
            }
            
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            if(user == null) // admin does not have a user; validation for admin
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "User not found" }
                        }
                    }));
            }

            if (!interestService.UserHasInterest(user, interest.Name))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "User does not have this interest." }
                        }
                    }));
            }

            interestService.RemoveInterestFromUser(interest.Name, user);
            return Ok(ServiceResult<Interest>.Success(interest));
        }

        [HttpGet("getUserInterests/{userId}")]
        public IActionResult GetUserInterests([FromRoute] int userId)
        {
            UserServices userServices = new UserServices();
            var user = userServices.GetById(userId);
            if(user == null)
            {
                return NotFound(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "User not found" }
                        }
                    }));
            }

            InterestServices service = new InterestServices();
            var interests = new List<Interest>();
            try
            {
                interests = service.GetUserInterests(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }

            var response = new InterestsGetResponse();    
            response.Items = interests;

            return Ok(ServiceResult<InterestsGetResponse>.Success(response));
                
        }
        #endregion
    }
}
