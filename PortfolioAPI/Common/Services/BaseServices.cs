using System;
using Common.Entities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class BaseServices<T> 
    where T : BaseEntity
{
    private DbContext Context { get; set; }
    private DbSet<T> Items { get; set; }

    public BaseServices()
    {
        Context = new AppDbContext();
        Items = Context.Set<T>();
    }
    // https://learn.microsoft.com/bg-bg/ef/core/change-tracking/
}
