using System;
using Common.Entities.UserDetailsEntities;

namespace Common.Entities;

public class SocialNetwork: BaseEntity
{
    public int UserId { get; set; }
    public string Type { get; set; }
    public string Account { get; set; }
    public string Link { get; set; }
}
