using System;
using System.Linq;
using Common.Entities;

namespace Common.Services;

public class HashtagServices : BaseServices<Hashtag>
{
    public Hashtag GetByTag(string tag)
    {
        return Items.FirstOrDefault(h => h.Tag == tag);
    }

    public void AddTagToPost(Hashtag hashtag, Post post)
    {
        if (!hashtag.Posts.Any(p => p.Id == post.Id) && !post.Hashtags.Any(h => h.Id == hashtag.Id))
        {
            hashtag.Posts.Add(post);
            post.Hashtags.Add(hashtag);
            Context.SaveChanges();
        }
    }

    public void RemoveTagFromPost(Hashtag hashtag, Post post)
    {
        if (hashtag.Posts.Any(p => p.Id == post.Id) && post.Hashtags.Any(h => h.Id == hashtag.Id))
        {
            hashtag.Posts.Remove(post);
            post.Hashtags.Remove(hashtag);
            Context.SaveChanges();
        }
    }
}
