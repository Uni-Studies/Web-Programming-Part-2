using System;
using System.Collections.Generic;
using Common.Entities;

namespace Common.Services;

public class ImageServices : BaseServices<Image>
{
    public void AddImageToPost(Image image, Post post)
    {
        //Context.Attach(post);
        image.Post = post;
        Context.SaveChanges();  
    }

    public void RemoveImageFromPost(Image image, Post post)
    {
        //Context.Attach(post);
        image.Post = null;
        Context.SaveChanges();
    }
}
