using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Common.Services;

 public class UsersServices : BaseService<User>
{
    #region Commented User Services
    // private DbContext DbContext { get; set; }
    // private DbSet<User> Items { get; set; }

    // public UsersServices()
    // {
    //     DbContext = new AppDbContext();
    //     Items = DbContext.Set<User>();
    // }
    // public List<User> GetAll(Expression<Func<User, bool>> filter = null, string orderBy = null, bool sortAsc = false, int page = 1, int itemsPerPage = int.MaxValue) // func is a delegate
    // {
    //     AppDbContext context = new AppDbContext();

    //     var query = context.Users.AsQueryable();
    //     if(filter != null)
    //         query = query.Where(filter);
        
    //     if(!string.IsNullOrEmpty(orderBy))
    //     {
    //         if(sortAsc)
    //         {
    //             query = query.OrderBy(e => EF.Property<object>(e, orderBy));
    //         }
    //         else
    //         {
    //             query = query.OrderByDescending(e => EF.Property<object>(e, orderBy));
    //         }
    //     }

    //     query = query.Skip((page - 1)* itemsPerPage)
    //                  .Take(itemsPerPage);
    //     return query.ToList();
    //     //return Items.ToList();
    //     //return context.Users.ToList();
    // }

    // public User GetById(int id)
    // {
    //     //AppDbContext context = new AppDbContext();
    //     var item = Items.FirstOrDefault(x => x.Id == id);
    //     return item;
    // }

    // public void Save(User item)
    // {
    //     //AppDbContext context = new AppDbContext();
    //     if (item.Id > 0)
    //     {
    //         Items.Update(item); // not mandatory
    //     }
    //     else
    //     {
    //         Items.Add(item);
    //     }
    //     DbContext.SaveChanges();
    // }
    
    // public void Delete(User item)
    // {
    //     //AppDbContext context = new AppDbContext();

    //     User forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
    //     if (forDelete == null)
    //         throw new Exception("Item not found");

    //     Items.Remove(forDelete);
    //     DbContext.SaveChanges();
    // }

    #endregion
}
