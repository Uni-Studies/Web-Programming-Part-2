using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Enums;

namespace Common.Services;

public class PostServices : BaseServices<Post>
{
    public void SavePost(User user, Post post)
    {
        Context.Attach(user);
        if (post.SavedByUsers.Contains(user))
        {
            throw new Exception("Post has already been saved!");
        }
        
        post.SavedByUsers.Add(user);
        post.SavesCount++;

        Context.SaveChanges();   
    }

    public void UnsavePost(User user, Post post)
    {
        Context.Attach(user);
        if (!post.SavedByUsers.Contains(user))
        {
            throw new Exception("Post is not saved!");
        }
        post.SavedByUsers.Remove(user);
        post.SavesCount--;

        Context.SaveChanges();
    }

    public List<Post> GetSavedPostsByUser(User user, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        var query = user.SavedPosts.AsEnumerable();
        
        if (!string.IsNullOrEmpty(orderBy))
        {
            var property = typeof(Post).GetProperty(orderBy);
            if (property != null)
            {
                query = sortAsc ? query.OrderBy(p => property.GetValue(p)) 
                                : query.OrderByDescending(p => property.GetValue(p));
            }
        }
        
        return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
}
