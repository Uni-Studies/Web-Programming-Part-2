using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class ProjectServices : BaseService<Project>
{
    #region Commented Services
    // private DbContext DbContext { get; set; }
    // private DbSet<Project> Items { get; set; }

    // public ProjectServices()
    // {
    //     DbContext = new AppDbContext();
    //     Items = DbContext.Set<Project>();
    // }
    //  public List<Project> GetAll(Expression<Func<Project, bool>> filter = null, string orderBy = null, bool sortAsc = false, int page = 1, int itemsPerPage = int.MaxValue) // func is a delegate
    // {
    //     AppDbContext context = new AppDbContext();

    //     var query = context.Projects.AsQueryable();
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
    //     return query.ToList();
    //     //return Items.ToList();
    //     //return context.Users.ToList();
    // }

    // public Project GetById(int id)
    // {
    //     //AppDbContext context = new AppDbContext();
    //     var item = Items.FirstOrDefault(x => x.Id == id);
    //     return item;
    // }

    // public void Save(Project item)
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
    
    // public void Delete(Project item)
    // {
    //     //AppDbContext context = new AppDbContext();

    //     Project forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
    //     if (forDelete == null)
    //         throw new Exception("Item not found");

    //     Items.Remove(forDelete);
    //     DbContext.SaveChanges();
    // }
    #endregion
}
