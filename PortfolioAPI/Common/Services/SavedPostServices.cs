using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class SavedPostServices : BaseServices<SavedPost>
{
    public void SavePost(int userId, int postId)
    {
        bool alreadySaved = Items.Any(sp => sp.UserId == userId && sp.PostId == postId);

        if (alreadySaved) return;

        var user = Context.Set<User>().FirstOrDefault(u => u.Id == userId);
        var post = Context.Set<Post>().FirstOrDefault(p => p.Id == postId);

        if (user == null || post == null)
            throw new Exception("User or Post not found.");

        SavedPost savedPost = new SavedPost
        {
            UserId = userId,
            PostId = postId,
            User = user,
            Post = post
        };

        Save(savedPost);
    }

    public void UnsavePost(int userId, int postId)
    {
        var saved = Items
            .FirstOrDefault(sp => sp.UserId == userId && sp.PostId == postId);

        if (saved != null)
        {
            Delete(saved);
        }
    }

    public List<Post> GetSavedPostsByUser(int userId)
    {
        var savedPosts = GetAll(sp => sp.UserId == userId).Select(sp => sp.Post).ToList();
        return savedPosts;
    }
}
