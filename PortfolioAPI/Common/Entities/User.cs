using System;
using System.Collections.Generic;
using Common.Entities.ManyToManyEntities;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Sex { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthCity { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public string Nationality { get; set; }
    public string Details { get; set; }
    public string ProfilePicture { get; set; }

    public virtual AuthUser AuthUser { get; set; }
    //public virtual List<UserSubmissionBaseEntity> UserSubmissions { get; set; }
    public virtual List<Post> Posts { get; set; }
    public virtual List<Post> SavedPosts { get; set; }
    public virtual List<SocialNetwork> SocialNetworks { get; set; }    
    public virtual List<Project> Projects { get; set; }
    public virtual List<Education> Educations { get; set; }
    public virtual List<Work> Jobs { get; set; }
    public virtual List<Course> Courses { get; set; } 
    public virtual List<UserSkill> UserSkills { get; set; }
}
