using System;
using System.Collections.Generic;
using Common.Entities;
using Common.Entities.DTOs;
using Common.Entities.ManyToManyEntities;

namespace API.Services;

public class FullUserExtension
{
    public FullUser UserBio { get; set; }
    public virtual List<Post> Posts { get; set; }
    public virtual List<Post> SavedPosts { get; set; }
    public virtual List<SocialNetwork> SocialNetworks { get; set; }   
    public virtual List<Project> Projects { get; set; }
    public virtual List<Education> Educations { get; set; }
    public virtual List<Work> Jobs { get; set; }
    public virtual List<Course> Courses { get; set; } 
    public virtual List<Event> Events { get; set; }
    public virtual List<Skill> UserSkills { get; set; }
}
