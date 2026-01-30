using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Entities;

public class Post : BaseEntity
{
    public int UserId { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public PostPrivacyLevel PrivacyLevel { get; set; }
    public User User { get; set; }
    public List<Image> Images { get; set; }
}
