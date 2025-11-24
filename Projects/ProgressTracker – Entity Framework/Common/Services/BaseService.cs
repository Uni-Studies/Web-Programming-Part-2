using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class BaseService<T> 
where T : BaseEntity
{
    private DbContext DbContext { get; set; }
    private DbSet<T> Items {get; set;}


    public BaseService()
    {
        DbContext = new AppDbContext();
        Items = DbContext.Set<T>();
    }

    // public List<T> GetAll()
    // {
    //     //AppDbContext context = new AppDbContext();
    //     return Items.ToList();
    // }
    
    public List<T> GetAll(Expression<Func<T, bool>> filter = null, string orderBy = null, bool sortAsc = false, int page = 1, int itemsPerPage = int.MaxValue) // func is a delegate
    {
        //AppDbContext context = new AppDbContext();

        var query = Items.AsQueryable();
        if(filter != null)
            query = query.Where(filter);
        
        if(!string.IsNullOrEmpty(orderBy))
        {
            if(sortAsc)
            {
                query = query.OrderBy(e => EF.Property<object>(e, orderBy));
            }
            else
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, orderBy));
            }
        }

        query = query.Skip((page - 1)* itemsPerPage)
                     .Take(itemsPerPage);
        return query.ToList();
        //return Items.ToList();
        //return context.Users.ToList();
    }
    
    public T GetById(int id)
    {
        //AppDbContext context = new AppDbContext();
        return Items.FirstOrDefault(x => x.Id == id);
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
