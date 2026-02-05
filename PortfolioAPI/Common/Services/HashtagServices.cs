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

    public List<Post> SearchPostsByHashtag(string hashtag, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        var tag = GetByTag(hashtag);

        if (tag is null)
            throw new Exception("Tag is not found!");

        Context.Attach(tag);
        var query = tag.Posts.AsEnumerable();

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
