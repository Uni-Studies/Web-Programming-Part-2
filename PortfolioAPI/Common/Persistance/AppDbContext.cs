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
    public DbSet<UserSkill> UserSkills { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(@"
                Data Source=(localdb)\MSSQLLocalDB;
                Database = PortfolioDb;
                User Id=alyavova;
                Password=alyavova;
                TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region EnumConversion
        modelBuilder.Entity<Post>().Property(p => p.PrivacyLevel).HasConversion<string>();
        modelBuilder.Entity<Project>().Property(s => s.CompletionStatus).HasConversion<string>();
        modelBuilder.Entity<Education>().Property(s => s.CompletionStatus).HasConversion<string>();
        modelBuilder.Entity<Course>().Property(s => s.CompletionStatus).HasConversion<string>();
        modelBuilder.Entity<Work>().Property(s => s.CompletionStatus).HasConversion<string>();
        modelBuilder.Entity<UserSkill>().Property(us => us.SubmissionType).HasConversion<string>();
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

        #region AuthUser
         modelBuilder.Entity<AuthUser>()
            .HasOne(au => au.User)
            .WithOne()
            .HasForeignKey<User>(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.SavedPosts)
            .WithMany(p => p.SavedByUsers)
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

                sp => sp.HasKey(t => new { t.UserId, t.PostId })
            );

        #endregion User
  
        #region Social Networks
        modelBuilder.Entity<SocialNetwork>()
            .HasOne(sn => sn.User)
            .WithMany(u => u.SocialNetworks)
            .HasForeignKey(sn => sn.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

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

        
        #region PostHashtag
        modelBuilder.Entity<Hashtag>()
            .HasMany(h => h.Posts)
            .WithMany(p => p.Hashtags);
        #endregion

        #region Projects
        modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects);

        #endregion

        #region Educations
        modelBuilder.Entity<Education>()
            .HasOne(e => e.User)
            .WithMany(u => u.Educations);
        #endregion

        #region Courses
        modelBuilder.Entity<Course>()
            .HasOne(c => c.User)
            .WithMany(u => u.Courses);
        #endregion

        #region Works/Jobs
        modelBuilder.Entity<Work>()
            .HasOne(w => w.User)
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
    }
}
