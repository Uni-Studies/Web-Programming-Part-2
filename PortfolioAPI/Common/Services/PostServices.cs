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
        post.SavesCount += 1;
        post.SavedByUsers ??= new List<User>();
        user.SavedPosts ??= new List<Post>();

        post.SavedByUsers.Add(user);
        user.SavedPosts.Add(post);
        
        Context.SaveChanges();
    }

    public void UnsavePost(User user, Post post)
    {
        post.SavesCount -= 1;
        post.SavedByUsers.Remove(user);
        user.SavedPosts.Remove(post);
        Context.SaveChanges();
    }

    public List<Post> GetSavedPostsByUser(int userId, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        Expression<Func<Post, bool>> filter = p => p.SavedByUsers.Any(u => u.Id == userId);
        var savedPosts = GetAll(filter, orderBy, sortAsc, page, pageSize).ToList();
        return savedPosts;
    }
}
