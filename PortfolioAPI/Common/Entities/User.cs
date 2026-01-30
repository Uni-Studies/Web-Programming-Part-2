using System;
using System.Collections.Generic;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class User : BaseEntity
{
    public int AuthUserId { get; set; }
    public string Sex { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthCity { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public string Nationality { get; set; }
    public string Details { get; set; }
    public string ProfilePicture { get; set; }

    public AuthUser AuthUser { get; set; }
    public List<UserSubmissionBaseEntity> UserSubmissions { get; set; } 
    public List<Skill> Skills { get; set; }
}
