using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using API.Infrastructure.RequestDTOs.Hashtag;
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
                    (model.Filter.CreatedBefore == null || p.CreatedAt <= model.Filter.CreatedBefore);
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
                    (p.PrivacyLevel == PostPrivacyLevel.Public) &&
                    (model.Filter.UserId == null || p.UserId == model.Filter.UserId);

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
            UserServices userServices = new UserServices();
            
            var filter = GetPublicPostsFilter(model);

            if(model.Filter.UserId != null)
            {
                var user = userServices.GetById((int)model.Filter.UserId);
                if(user == null)
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
            }

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

        [Authorize]
        [HttpGet("getUserAllPosts")]
        public IActionResult GetUserAllPosts([FromQuery] PostsGetRequest model)
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
            var filter = GetFilter(model);

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
        [HttpDelete("unsavePost/{postId}")]
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
        [HttpPost("addHashtag/{postId}")]
        public IActionResult AddHashtag([FromRoute] int postId, [FromBody] HashtagRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Tag))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Tag",
                            Messages = new List<string> { "Tag is required" }
                        }
                    }));
            }

            model.Tag = model.Tag.Trim().ToLower();
            
            HashtagServices hashtagService = new HashtagServices();
            var hashtag = hashtagService.GetByTag(model.Tag);

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

            if (hashtag is not null && hashtagService.PostHasTag(post, hashtag.Tag))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "Post already has this hashtag." }
                        }
                    }));
            }

            if(hashtag is null)
            {
                hashtag = new Hashtag() { Tag = model.Tag };
                hashtagService.Save(hashtag);
            }
            
            hashtagService.AddTagToPost(hashtag.Tag, post);
            return Ok(ServiceResult<Post>.Success(post));
        }
        
        [Authorize]
        [HttpDelete("removeHashtag/{postId}")]
        public IActionResult RemoveHashtag([FromBody] HashtagRequest model, [FromRoute] int postId)
        {
            if (string.IsNullOrWhiteSpace(model.Tag))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Tag",
                            Messages = new List<string> { "Tag is required" }
                        }
                    }));
            }

            model.Tag = model.Tag.Trim().ToLower();
            HashtagServices hashtagService = new HashtagServices();
            
            var hashtag = hashtagService.GetByTag(model.Tag);
            if(hashtag == null)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Hashtag not found" }
                        }
                    }));
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

            if (!hashtagService.PostHasTag(post, hashtag.Tag))
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Global",
                            Messages = new List<string> { "Post does not have this hashtag." }
                        }
                    }));
            }

            hashtagService.RemoveTagFromPost(hashtag.Tag, post);
            return Ok(ServiceResult<Post>.Success(post));
        }

        [HttpGet("getPostsByHashtag")]
        public IActionResult GetPostsByHashtag([FromQuery] PostsGetRequest model)
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

            HashtagServices service = new HashtagServices();

            var foundPosts = new List<Post>();
            try
            {
                foundPosts = service.SearchPostsByHashtag(model.Filter.Hashtag);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Global", ex.Message);
                return BadRequest(
                    ServiceResultExtension<List<Error>>.Failure(null, ModelState)
                );
            }

            var response = new PostsGetResponse();    

            response.Pager = new PagerResponse();
            response.Pager.Page = model.Pager.Page;
            response.Pager.PageSize = model.Pager.PageSize;
            response.OrderBy = model.OrderBy;
            response.SortAscending = model.SortAscending;

            PopulateGetResponse(model, response);

            response.Pager.Count = foundPosts.Count;
            response.Items = foundPosts;

            return Ok(ServiceResult<PostsGetResponse>.Success(response));
                
        }
        
        
        #endregion

        #region Image Management
        [Authorize]
        [HttpPost("addImageToPost/{postId}")]
        public IActionResult AddImageToPost([FromRoute] int postId, [FromForm] IFormFile image)
        {
            if(image == null || image.Length == 0)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Image is required" }
                        }
                    }));
            }
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

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            ImageServices imageServices = new ImageServices();
            Image imageEntity = new Image { ImagePath = fileName, PostId = post.Id };
            imageServices.Save(imageEntity);
            imageServices.AddImageToPost(imageEntity, post);
            
            return Ok(ServiceResult<Image>.Success(imageEntity));
        }

        [Authorize]
        [HttpDelete("removeImageFromPost/{imageId}/{postId}")]
        public IActionResult RemoveImageFromPost([FromRoute] int imageId, [FromRoute] int postId)
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
            var image = imageServices.GetById(imageId);

            if(image == null || image.PostId != post.Id)
            {
                return BadRequest(ServiceResult<Post>.Failure(null,
                    new List<Error>
                    {
                        new Error()
                        {
                            Key = "Global",
                            Messages = new List<string>() { "Image not found for this post" }
                        }
                    }));
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", image.ImagePath);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            imageServices.RemoveImageFromPost(image, post);
            imageServices.Delete(image);
            
            return Ok(ServiceResult<Image>.Success(image));
        }
        #endregion
    }
}
