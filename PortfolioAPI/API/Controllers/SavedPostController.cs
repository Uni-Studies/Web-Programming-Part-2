using System;
using Common.Entities.ManyToManyEntities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedPostController : ControllerBase
    {
        [HttpPost("save")]
        public IActionResult SavePost(int postId) 
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            SavedPostServices service = new SavedPostServices();
            service.SavePost(loggedUserId, postId);

            return Ok();
        }

        [HttpPost("unsave")]
        public IActionResult UnsavePost(int postId)
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            SavedPostServices service = new SavedPostServices();
            service.UnsavePost(loggedUserId, postId);

            return Ok();
        }

        [HttpGet("savedPosts")]
        public IActionResult GetSavedPosts()
        {
            int loggedUserId = Convert.ToInt32(this.User.FindFirst("loggedUserId").Value);

            SavedPostServices service = new SavedPostServices();
            var savedPosts = service.GetSavedPostsByUser(loggedUserId);

            return Ok(savedPosts);
        }
    }
}
