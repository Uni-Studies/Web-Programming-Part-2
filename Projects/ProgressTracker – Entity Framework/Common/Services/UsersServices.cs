using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UsersServices : BaseService<User>
{
    // private DbContext DbContext { get; set; }
    // private DbSet<User> Items { get; set; }

    // public UsersServices()
    // {
    //     DbContext = new AppDbContext();
    //     Items = DbContext.Set<User>();
    // }
    // public List<User> GetAll()
    // {
    //     //AppDbContext context = new AppDbContext();
    //     return Items.ToList();
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
}
