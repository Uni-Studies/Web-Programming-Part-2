using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class BaseService<T> 
where T : BaseEntity
{
    private DbContext DbContext { get; set; }
    private DbSet<T> Items { get; set; }

    public BaseService()
    {
        DbContext = new AppDbContext();
        Items = DbContext.Set<T>();
    }

    public List<T> GetAll()
    {
        //AppDbContext context = new AppDbContext();
        return Items.ToList();
    }

    public T GetById(int id)
    {
        //AppDbContext context = new AppDbContext();
        var item = Items.FirstOrDefault(x => x.Id == id);
        return item;
    }

    public void Save(T item)
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
    
    public void Delete(T item)
    {
        //AppDbContext context = new AppDbContext();

        T forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
        if (forDelete == null)
            throw new Exception("Item not found");

        Items.Remove(forDelete);
        DbContext.SaveChanges();
    }
}
