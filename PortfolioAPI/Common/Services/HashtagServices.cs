using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;

namespace Common.Services;

public class HashtagServices : BaseServices<Hashtag>
{
    public Hashtag GetByTag(string tag)
    {
        return Items.FirstOrDefault(h => h.Tag == tag);
    }

    public bool PostHasTag(Post post, string tag)
    {
        Context.Attach(post);
        return post.Hashtags.Any(t => t.Tag.Equals(tag));
    }
    public void AddTagToPost(string tag, Post post)
    {
        Context.Attach(post);

        var hashtag = GetByTag(tag);
        post.Hashtags.Add(hashtag);
        
        Context.SaveChanges();
    }

    public void RemoveTagFromPost(string tag, Post post)
    {
        Context.Attach(post);

        var hashtag = GetByTag(tag);
        post.Hashtags.Remove(hashtag);
        
        Context.SaveChanges();
    }

    public List<Post> SearchPostsByHashtag(string hashtag)
    {
        var tag = GetByTag(hashtag);

        if(tag is null)
            throw new Exception("Tag is not found!");
        Context.Attach(tag);
        return tag.Posts; 
    }
}
