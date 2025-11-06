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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the database connection string
        optionsBuilder
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
    }
}
