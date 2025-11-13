using System;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common;

public class AppDbContext : DbContext // DbContext in Microsoft.EntityFrameworkCore
{
    //Microsoft.EntityFrameworkCore
    //Microsoft.EntityFrameworkCore.SqlServer
    //Microsoft.EntityFrameworkCore.Design

    public DbSet<User> Users { get; set; } // represents the Users table in the database
    public DbSet<Project> Projects { get; set; } 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the database connection string
        optionsBuilder
            .UseLazyLoadingProxies() // enables lazy loading
            .UseSqlServer(@"
                Server=(localdb)\MSSQLLocalDB;
                Database=PTDB;
                User Id=alyavova;
                Password=alyavova;
                TrustServerCertificate=True;"
            );

        // creates a migration script that would change the database to match the current state of the DbContext
        // dotnet ef migrations add Initial

        // ouputs the SQL script that is generated from the Migrations
        // dotnet ef migrations script

        // updates the database from the Migration scripts
        // dotnet ef database update
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id); 

        // Configure entity properties and relationships if needed
        modelBuilder.Entity<User>()
                         .HasData(new User
                         {
                             Id = 1,
                             FirstName = "Admin",
                             LastName = "Adminov",
                             Username = "adminpass",
                             Password = "admin"
                         });
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Project>()
            .HasOne<User>(p => p.Owner) // each project has one owner
            .WithMany(/*u => u.Projects*/)    // each user can own multiple projects
            .HasForeignKey(p => p.OwnerId) // foreign key in Projects table
            .OnDelete(DeleteBehavior.Restrict); // when a User is deleted, their Projects are also deleted
              // otherwise not consistent behaviour of the database
              // do not do cascade behaviour in delete    
    }
}
