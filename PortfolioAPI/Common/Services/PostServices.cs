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
    public void SavePost(int userId, int postId)
    {
        bool alreadySaved = Context.Set<SavedPost>()
        .Any(sp => sp.UserId == userId && sp.PostId == postId);

        if (alreadySaved) return;

        var user = Context.Set<User>()
            .Where(u => u.Id == userId)
            .FirstOrDefault();

        var post = Items
            .Where(p => p.Id == postId)
            .FirstOrDefault();

        if (user == null || post == null)
        {
            throw new Exception("User or Post not found.");
        }

        if (!post.SavedByUsers.Contains(user) && !user.SavedPosts.Contains(post))
        {
            post.SavedByUsers.Add(user);
            user.SavedPosts.Add(post);
            Context.Set<SavedPost>().Add(new SavedPost
            {
                UserId = userId,
                PostId = postId
            });
            Context.SaveChanges();
        }
    }
}
