using System;
using System.Data.Common;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Entities.UserDetailsEntities;
using Common.Entities.UserSubmissionsEntities;
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
    public DbSet<Hashtag> Hashtags { get; set; }

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
        #region BaseEntity
        modelBuilder.Entity<BaseEntity>()
            .HasKey(be => be.Id);
        #endregion

        #region AuthUser
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
            .HasOne(u => u.AuthUser)
            .WithOne()
            .HasForeignKey<User>(u => u.AuthUserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion User

        #region Post
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Hashtags)
            .WithMany(h => h.Posts);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Post)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion  

        #region Image
        
        #endregion

        #region Social Networks
        modelBuilder.Entity<SocialNetwork>()
            .HasOne(sn => sn.User)
            .WithMany()
            .HasForeignKey(sn => sn.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region UserSubmissionBaseEntity
        modelBuilder.Entity<UserSubmissionBaseEntity>()
            .HasMany(ud => ud.Users)
            .WithMany()
            .UsingEntity<UserSubmission>(
                us => us
                        .HasOne(us => us.User)
                        .WithMany()
                        .HasForeignKey(us => us.UserId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => us
                        .HasOne(us => us.Submission)
                        .WithMany()
                        .HasForeignKey(us => us.SubmissionId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => 
                    us.HasKey(t => new { t.UserId, t.SubmissionId })
            );
        #endregion

        #region Skills
        modelBuilder.Entity<Skill>()
            .HasMany(u => u.Users)
            .WithMany()
            .UsingEntity<UserSkill>(
                us => us
                        .HasOne(us => us.User)
                        .WithMany()
                        .HasForeignKey(us => us.UserId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => us    
                        .HasOne(us => us.Skill)
                        .WithMany()
                        .HasForeignKey(us => us.SkillId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => 
                    us.HasKey(t => new { t.UserId, t.SkillId })
            );

        modelBuilder.Entity<Skill>()
            .HasMany(u => u.Sources)
            .WithMany()
            .UsingEntity<SubmissionSkill>(
                us => us
                        .HasOne(us => us.Submission)
                        .WithMany()
                        .HasForeignKey(us => us.SubmissionId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => us    
                        .HasOne(us => us.Skill)
                        .WithMany()
                        .HasForeignKey(us => us.SkillId)
                        .OnDelete(DeleteBehavior.Restrict),
                us => 
                    us.HasKey(t => new { t.SubmissionId, t.SkillId })
            );
        #endregion
    }
}
