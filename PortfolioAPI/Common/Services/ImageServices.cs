using System;
using System.Collections.Generic;
using Common.Entities;

namespace Common.Services;

public class ImageServices : BaseServices<Image>
{
    public void AddImageToPost(Image image, Post post)
    {
        post.Images.Add(image);
        image.Post = post;
        Save(image);  
    }

    public void RemoveImageFromPost(Image image, Post post)
    {
        post.Images.Remove(image);
        image.Post = null;
        Save(image);  
    }
}
