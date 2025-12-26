using System;
using System.Data.Common;
using Common.Entities;
using Microsoft.EntityFrameworkCore;


namespace Common.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<AuthUser> AuthUsers { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<SocialNetwork> SocialNetworks { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Work> Works { get; set; }
    public DbSet<Skill> Skills { get; set; }
}
