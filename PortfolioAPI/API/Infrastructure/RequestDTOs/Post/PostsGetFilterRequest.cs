using System;

namespace API.Infrastructure.RequestDTOs.Post;

public class PostsGetFilterRequest
{
    public int? UserId { get; set; }
    public string Location { get; set; }    
    public string DescriptionContains { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public string Hashtag { get; set; }
}
