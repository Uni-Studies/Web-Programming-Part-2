using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class ProjectServices
{
    private DbContext DbContext { get; set; }
    private DbSet<Project> Items { get; set; }

    public ProjectServices()
    {
        DbContext = new AppDbContext();
        Items = DbContext.Set<Project>();
    }
    public List<Project> GetAll()
    {
        AppDbContext context = new AppDbContext();
        //return Items.ToList();
        return context.Projects.ToList();
    }

    public Project GetById(int id)
    {
        //AppDbContext context = new AppDbContext();
        var item = Items.FirstOrDefault(x => x.Id == id);
        return item;
    }

    public void Save(Project item)
    {
        //AppDbContext context = new AppDbContext();
        if (item.Id > 0)
        {
            Items.Update(item); // not mandatory
        }
        else
        {
            Items.Add(item);
        }
        DbContext.SaveChanges();
    }
    
    public void Delete(Project item)
    {
        //AppDbContext context = new AppDbContext();

        Project forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
        if (forDelete == null)
            throw new Exception("Item not found");

        Items.Remove(forDelete);
        DbContext.SaveChanges();
    }
}
