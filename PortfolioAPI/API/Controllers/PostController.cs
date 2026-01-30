using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using API.Infrastructure.RequestDTOs.Post;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.ResponseDTOs.Post;
using API.Infrastructure.ResponseDTOs.Shared;
using Azure.Core.Pipeline;
using Common;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Enums;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseCRUDController<Post, PostServices, PostRequest, PostsGetRequest, PostsGetResponse>
    {
        protected override void PopulateEntity(Post item, PostRequest model, out string error)
        {
            error = null; 
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            item.UserId = loggedUserId;
            item.Location = model.Location;
            item.Description = model.Description;
            item.CreatedAt = model.CreatedAt;
            item.LikesCount = model.LikesCount;
            item.PrivacyLevel = model.PrivacyLevel;
            item.Images = model.Images;
            item.Hashtags = model.Hashtags; 
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
                    (model.Filter.Hashtag == null || p.Hashtags.Any(h => h.Tag == model.Filter.Hashtag)) &&
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
                    (model.Filter.Hashtag == null || p.Hashtags.Any(h => h.Tag == model.Filter.Hashtag)) &&
                    (p.PrivacyLevel == PostPrivacyLevel.Public);

        }

        protected override void PopulateGetResponse(PostsGetRequest request, PostsGetResponse response)
        {
            response.Filter = request.Filter;
        }

        [HttpGet]
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
    }
}
