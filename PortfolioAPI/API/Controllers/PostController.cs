using System;
using System.Linq;
using System.Linq.Expressions;
using API.Infrastructure.RequestDTOs.Post;
using API.Infrastructure.ResponseDTOs.Post;
using Azure.Core.Pipeline;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    (model.Filter.UserId == null || p.UserId == model.Filter.UserId);
        }

        protected override void PopulateGetResponse(PostsGetRequest request, PostsGetResponse response)
        {
            response.Filter = request.Filter;
        }

        
    }
}
