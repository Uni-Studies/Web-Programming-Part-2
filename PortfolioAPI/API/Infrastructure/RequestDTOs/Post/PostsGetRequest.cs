using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Post;

public class PostsGetRequest : BaseGetRequest
{
    public PostsGetFilterRequest Filter { get; set; }
}
