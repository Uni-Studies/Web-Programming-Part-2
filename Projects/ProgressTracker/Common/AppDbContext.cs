using System;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common;

public class AppDbContext : DbContext // DbContext in Microsoft.EntityFrameworkCore
{
    //Microsoft.EntityFrameworkCore
    //Microsoft.EntityFrameworkCore.SqlServer
    //Microsoft.EntityFrameworkCore.Design

    public  DbSet<User> Users { get; set; } // represents the Users table in the database
}
