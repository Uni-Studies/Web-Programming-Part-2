using System;
using System.Data.Common;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
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

    public DbSet<SavedPost> SavedPosts { get; set; }
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
                Username = "admin",
                Email = "stu2401321005@uni-plovdiv.bg",
                Password = "adminpass"
            });
        #endregion

        #region User
         modelBuilder.Entity<User>()
            .HasOne(u => u.AuthUser)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.SavedPosts)
            .WithMany()
            .UsingEntity<SavedPost>(
                sp => sp
                        .HasOne(sp => sp.Post)
                        .WithMany()
                        .HasForeignKey(sp => sp.PostId)
                        .OnDelete(DeleteBehavior.Cascade),
                sp => sp    
                        .HasOne(sp => sp.User)
                        .WithMany()
                        .HasForeignKey(sp => sp.UserId)
                        .OnDelete(DeleteBehavior.Cascade),
                sp => 
                    sp.HasKey(t => new { t.UserId, t.PostId })
            );

        #endregion User

        #region Post
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Image
        modelBuilder.Entity<Image>()
            .HasOne(i => i.Post)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Social Networks
        modelBuilder.Entity<SocialNetwork>()
            .HasOne(sn => sn.User)
            .WithMany(u => u.SocialNetworks)
            .HasForeignKey(sn => sn.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Projects
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Users)
            .WithMany(u => u.Projects);

        #endregion

        #region Educations
        modelBuilder.Entity<Education>()
            .HasMany(e => e.Users)
            .WithMany(u => u.Educations);
        #endregion

        #region Courses
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Courses);
        #endregion

        #region Works/Jobs
        modelBuilder.Entity<Work>()
            .HasMany(w => w.Users)
            .WithMany(u => u.Jobs);
        #endregion

        #region UserSkills
        modelBuilder.Entity<UserSkill>()
            .HasKey(us => new { us.UserId, us.SkillId, us.SubmissionType, us.SubmissionId });

        modelBuilder.Entity<UserSkill>()
            .HasOne(us => us.User)
            .WithMany(u => u.UserSkills)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserSkill>()
            .HasOne(us => us.Skill)
            .WithMany(s => s.UserSkills)
            .HasForeignKey(us => us.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region PostHashtag
        modelBuilder.Entity<Hashtag>()
            .HasMany(h => h.Posts)
            .WithMany(p => p.Hashtags);
        #endregion
    }
}
