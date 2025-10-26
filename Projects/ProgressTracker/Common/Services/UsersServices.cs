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
            },
            new User()
            {
                Id = 2,
                Username = "john_doe",
                Password = "jdpassword",
                FirstName = "John",
                LastName = "Doe"
            }
            // property initializers
        };
    }

    public List<User> GetAll()
    {
        return Items;
    }

    public User GetById(int id)
    {
        return Items.FirstOrDefault(x => x.Id == id);
    }

    public void Save(User item)
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
    }
    
    public void Delete(User item)
    {
        User forDelete = Items.FirstOrDefault(x => x.Id == item.Id);
        if (forDelete == null)
            throw new Exception("Item not found");
        Items.Remove(forDelete);
    }
}
