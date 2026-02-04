using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Enums;

namespace Common.Services;

public class PostServices : BaseServices<Post>
{
    public void SavePost(User user, Post post)
    {
        if (post.SavedByUsers.Contains(user))
        {
            throw new ArgumentException("Post has already been saved!");
        }
        
        Context.Attach(user);
        post.SavedByUsers.Add(user);
        post.SavesCount++;

        Context.SaveChanges();   
    }

    public void UnsavePost(User user, Post post)
    {
        Context.Attach(user);
        post.SavedByUsers.Remove(user);
        post.SavesCount--;

        Context.SaveChanges();
    }

    public List<Post> GetSavedPostsByUser(User user, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        return user.SavedPosts; 
    }
}
