using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Entities;

public class Post : BaseEntity
{
    public int UserId { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public PostPrivacyLevel PrivacyLevel { get; set;}
    public virtual User User { get; set; }
    public virtual List<Image> Images { get; set; }
    public virtual List<Hashtag> Hashtags { get; set; }
    public virtual List<User> SavedByUsers { get; set; }
}
