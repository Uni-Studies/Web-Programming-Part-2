using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Common.Entities;

namespace Common.Services;

public class UsersServices
{
    private static List<User> Items { get; set; }

    static UsersServices()
    {
        Items = new List<User>()
        {
            new User()
            {
                Id = 1,
                Username = "alyavova",
                Password = "aneliyapass",
                FirstName = "Aneliya",
                LastName = "Lyavova"
            }
            // property initializers
        };
    }

    public ServiceResult<List<User>> GetAll()
    {
        if (Items == null)
        {
            return ServiceResult<List<User>>.Failure(null, new List<Error>()
            {
                new Error()
                {
                    Key = "Global",
                    Messages = new List<string>() { "No users found" }
                }
            });
        }
            
        return ServiceResult<List<User>>.Success(Items);
    }

    public ServiceResult<User> GetById(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return ServiceResult<User>.Failure(new User {Id = id}, new List<Error>()
            {
                new Error()
                {
                    Key = "Global",
                    Messages = new List<string>() { "User not found" }
                }
            });
        }
        return ServiceResult<User>.Success(item);
    }

    public ServiceResult<User> Save(User item)
    {
        if (item.Id > 0)
        {
            User forUpdate = Items.FirstOrDefault(x => x.Id == item.Id);
            if (forUpdate == null)
                throw new Exception("Item not found");
            forUpdate.Username = item.Username;
            forUpdate.Password = item.Password;
            forUpdate.FirstName = item.FirstName;
            forUpdate.LastName = item.LastName;
        }
        else
        {
            if (Items.Count <= 0)
                item.Id = 1;
            else
                item.Id = Items.Max(x => x.Id) + 1;
            Items.Add(item);

            /*
            Items.Index = items.Count <= 0
                                    ? 1
                                    : Items.Max(x => x.Id) + 1;
            */
        }
        if (!Items.Contains(item))
            return ServiceResult<User>.Failure(item, new List<Error>()
            {
                new Error()
                {
                    Key = "Global",
                    Messages = new List<string>() { "Operation failed" }
                }
            });
             
        return ServiceResult<User>.Success(item);   
    }
    
    public ServiceResult<User> Delete(User item)
    {
        // User forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
        // if (forDelete == null)
        //     throw new Exception("Item not found");
        // Items.Remove(forDelete);
        Items.Remove(item);
        if (Items.Contains(item))
        {
            return ServiceResult<User>.Failure(item, new List<Error>()
            {
                new Error()
                {
                    Key = "Global",
                    Messages = new List<string>() { "Operation Delete failed" }
                }
            });
        }
        return ServiceResult<User>.Success(item);
    }
}
