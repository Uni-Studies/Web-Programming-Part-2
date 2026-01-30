using System;
using API.Infrastructure.RequestDTOs.Post;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Post;

public class PostsGetResponse : BaseGetResponse<Common.Entities.Post>
{
    public PostsGetFilterRequest Filter { get; set; }
}
