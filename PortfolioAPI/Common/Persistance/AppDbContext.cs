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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(@"
                Data Source=(localdb)\MSSQLLocalDB;
                Database = PortfolioDb;
                User Ud=alyavova;
                Password=alyavova;
                TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region AuthUser
        modelBuilder.Entity<AuthUser>()
            .HasKey(au => au.Id);

        modelBuilder.Entity<AuthUser>()
            .HasData(new AuthUser
            {
                Id = 1, 
                FirstName = "admin",
                LastName = "admin",
                Username = "admin",
                Email = "stu2401321005@uni-plovdiv.bg",
                Password = "adminpass"
            });
        #endregion

        #region User
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        
    }
}
