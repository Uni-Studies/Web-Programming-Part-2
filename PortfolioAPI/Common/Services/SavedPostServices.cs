using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class SavedPostServices : BaseServices<SavedPost>
{
    public void SavePost(User user, Post post)
    {
        post.LikesCount += 1;
        post.SavedByUsers.Add(user);
        user.SavedPosts.Add(post);
    }

    public void UnsavePost(int userId, int postId)
    {
        var saved = Items
            .FirstOrDefault(sp => sp.UserId == userId && sp.PostId == postId);

        Delete(saved);
        if (saved != null)
        {
            
        }
    }

    public List<Post> GetSavedPostsByUser(int userId, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        Expression<Func<SavedPost, bool>> filter = sp => sp.UserId == userId;
        var savedPosts = GetAll(filter, orderBy, sortAsc, page, pageSize).Select(sp => sp.Post).ToList();
        return savedPosts;
    }
}
