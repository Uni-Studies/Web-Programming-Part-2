using System;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<WorkLog> WorkLogs { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(@"
                Server=localhost\SQLEXPRESS;
                Database=PTDB;
                User Id=nvalchanov;
                Password=nikipass;
                TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "adminpass",
                FirstName = "Admini",
                LastName = "Strator"
            });

        #endregion

        #region Project

        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Members)
            .WithMany()
            .UsingEntity<ProjectMember>(
                pm => pm
                        .HasOne(pm => pm.User)
                        .WithMany()
                        .HasForeignKey(pm => pm.UserId)
                        .OnDelete(DeleteBehavior.Restrict),
                pm => pm
                        .HasOne(pm => pm.Project)
                        .WithMany()
                        .HasForeignKey(pm => pm.ProjectId)
                        .OnDelete(DeleteBehavior.Restrict),
                pm =>
                    pm.HasKey(t => new { t.ProjectId, t.UserId })
            );

        #endregion

        #region Task

        modelBuilder.Entity<Task>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Owner)
            .WithMany()
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Assignee)
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region WorkLog

        modelBuilder.Entity<WorkLog>()
            .HasKey(w => w.Id);

        modelBuilder.Entity<WorkLog>()
            .HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkLog>()
            .HasOne(w => w.Task)
            .WithMany()
            .HasForeignKey(w => w.TaskId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion
    }
}
