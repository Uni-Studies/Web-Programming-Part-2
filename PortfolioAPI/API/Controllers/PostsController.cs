using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using API.Infrastructure.RequestDTOs.Post;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Post;
using API.Infrastructure.ResponseDTOs.Shared;
using API.Services;
using Azure.Core.Pipeline;
using Common;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Enums;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : BaseCRUDController<Post, PostServices, PostRequest, PostsGetRequest, PostsGetResponse>
    {
        protected override void PopulateEntity(Post item, PostRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.Location = model.Location;
            item.Description = model.Description;
            item.CreatedAt = model.CreatedAt;
            item.SavesCount = model.SavesCount;
            item.PrivacyLevel = model.PrivacyLevel;
        }

        protected override Expression<Func<Post, bool>> GetFilter(PostsGetRequest model)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            model.Filter ??= new PostsGetFilterRequest();

            return 
                p => 
                    (p.UserId == loggedUserId) &&
                    (string.IsNullOrEmpty(model.Filter.Location) || p.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.DescriptionContains) || p.Description.Contains(model.Filter.DescriptionContains)) &&
                    (model.Filter.CreatedAfter == null || p.CreatedAt >= model.Filter.CreatedAfter) &&
                    (model.Filter.CreatedBefore == null || p.CreatedAt <= model.Filter.CreatedBefore) &&
                    //(model.Filter.Hashtag == null || p.Hashtags.Any(h => h.Tag == model.Filter.Hashtag)) &&
                    (model.Filter.UserId == null || p.UserId == model.Filter.UserId) &&
                    (p.PrivacyLevel == PostPrivacyLevel.Public);
        }

        protected Expression<Func<Post, bool>> GetPublicPostsFilter(PostsGetRequest model)
        {
            model.Filter ??= new PostsGetFilterRequest();

            return 
                p => 
                    (string.IsNullOrEmpty(model.Filter.Location) || p.Location.Contains(model.Filter.Location)) &&
                    (string.IsNullOrEmpty(model.Filter.DescriptionContains) || p.Description.Contains(model.Filter.DescriptionContains)) &&
                    (model.Filter.CreatedAfter == null || p.CreatedAt >= model.Filter.CreatedAfter) &&
                    (model.Filter.CreatedBefore == null || p.CreatedAt <= model.Filter.CreatedBefore) &&
                    //(model.Filter.Hashtag == null || p.Hashtags.Any(h => h.Tag == model.Filter.Hashtag)) &&
                    (p.PrivacyLevel == PostPrivacyLevel.Public);

        }

        protected override void PopulateGetResponse(PostsGetRequest request, PostsGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [HttpGet("getPublicPosts")]
        public IActionResult GetPublicPosts([FromQuery] PostsGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;

            model.OrderBy ??= nameof(BaseEntity.Id);
            
            model.OrderBy = nameof(Common.Entities.Post.CreatedAt);
            model.SortAscending = false;

            var service = new PostServices();
            var filter = GetPublicPostsFilter(model);

            var response = new PostsGetResponse
            {
                Pager = new PagerResponse
                {
                    Page = model.Pager.Page,
                    PageSize = model.Pager.PageSize,
                    Count = service.Count(filter)
                },
                OrderBy = model.OrderBy,
                SortAscending = model.SortAscending,
            };

            PopulateGetResponse(model, response);

            response.Items = service.GetAll(
                filter,
                model.OrderBy,
                model.SortAscending,
                model.Pager.Page,
                model.Pager.PageSize
            );

            return Ok(ServiceResult<PostsGetResponse>.Success(response));
        }

        #region Save/Unsave Post
        [Authorize]
        [HttpPost("savePost/{postId}")]

        public IActionResult SavePost([FromRoute]int postId) 
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            PostServices postServices = new PostServices();
            var post = postServices.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }

            try
            {
                postServices.SavePost(user, post); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }
            
            return Ok(ServiceResult<Post>.Success(post));
        }

        [Authorize]
        [HttpPost("unsavePost/{postId}")]
        public IActionResult UnsavePost([FromRoute]int postId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            PostServices postServices = new PostServices();
            var post = postServices.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }

            postServices.UnsavePost(user, post);
            
            return Ok(ServiceResult<Post>.Success(post));
        }

        [Authorize]
        [HttpGet("getSavedPosts")]
        public IActionResult GetSavedPosts([FromQuery] PostsGetRequest model)
        {
            model.Pager = model.Pager ?? new PagerRequest();
            model.Pager.Page = model.Pager.Page <= 0
                                    ? 1
                                    : model.Pager.Page;
            model.Pager.PageSize = model.Pager.PageSize <= 0
                                        ? 10
                                        : model.Pager.PageSize;
            model.OrderBy ??= nameof(BaseEntity.Id);
            model.OrderBy = typeof(Post).GetProperty(model.OrderBy) != null
                                ? model.OrderBy
                                : nameof(BaseEntity.Id);
                                
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            UserServices userServices = new UserServices();
            var user = userServices.GetById(loggedUserId);

            PostServices service = new PostServices();
            var savedPosts = service.GetSavedPostsByUser(user);

            Expression<Func<Post, bool>> filter = GetFilter(model);

            var response = new PostsGetResponse();    

            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;
            response.OrderBy = model.OrderBy;
            response.SortAscending = model.SortAscending;

            PopulateGetResponse(model, response);

            response.Pager.Count = savedPosts.Count;
            response.Items = savedPosts;

            return Ok(ServiceResult<PostsGetResponse>.Success(response));
        }
        
        #endregion



        #region Hashtag Management
        [Authorize]
        [HttpPost("addHashtag")]
        public IActionResult AddHashtag([FromBody] string tag, [FromRoute] int postId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            HashtagServices hashtagService = new HashtagServices();
            var hashtag = hashtagService.GetByTag(tag);
            if(hashtag == null)
            {
                hashtagService.Save(new Hashtag { Tag = tag });
                hashtag = hashtagService.GetByTag(tag);
            }

            PostServices postService = new PostServices();
            var post = postService.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }
            hashtagService.AddTagToPost(hashtag, post);
            return Ok(ServiceResult<Post>.Success(post));
        }
        
        [Authorize]
        [HttpPost("removeHashtag")]
        public IActionResult RemoveHashtag([FromBody] string tag, [FromRoute] int postId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);
            HashtagServices hashtagService = new HashtagServices();
            var hashtag = hashtagService.GetByTag(tag);
            if(hashtag == null)
            {
                hashtagService.Save(new Hashtag { Tag = tag });
                hashtag = hashtagService.GetByTag(tag);
            }

            PostServices postService = new PostServices();
            var post = postService.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }
            hashtagService.RemoveTagFromPost(hashtag, post);
            return Ok(ServiceResult<Post>.Success(post));
        }
        #endregion

        #region Image Management
        [Authorize]
        [HttpPost("addImageToPost")]
        public IActionResult AddImageToPost([FromBody] Image image, [FromRoute] int postId)
        {
            PostServices postServices = new PostServices();
            var post = postServices.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }

            ImageServices imageServices = new ImageServices();
            imageServices.AddImageToPost(image, post);
            
            return Ok(ServiceResult<Image>.Success(image));
        }

        [Authorize]
        [HttpPost("removeImageFromPost")]
        public IActionResult RemoveImageFromPost([FromBody] Image image, [FromRoute] int postId)
        {
            PostServices postServices = new PostServices();
            var post = postServices.GetById(postId);
            if(post == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Post not found" }
                        }
                    }));
            }

            ImageServices imageServices = new ImageServices();
            imageServices.RemoveImageFromPost(image, post);
            
            return Ok(ServiceResult<Image>.Success(image));
        }
        #endregion
    }
}
