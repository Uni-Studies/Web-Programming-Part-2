using System;
using System.Collections.Generic;
using Common.Entities;
using Common.Enums;

namespace API.Infrastructure.RequestDTOs.Post;

public class PostRequest
{
    public int UserId { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public PostPrivacyLevel PrivacyLevel { get; set; }
    
}
