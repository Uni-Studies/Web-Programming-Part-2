using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Hashtag;

public class HashtagGetRequest : BaseGetRequest
{
    public HashtagGetFilterRequest Filter { get; set; }
}
