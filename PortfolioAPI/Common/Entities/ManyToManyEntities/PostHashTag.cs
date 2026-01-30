using System;

namespace Common.Entities.ManyToManyEntities;

public class PostHashTag
{
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int HashtagId { get; set; }
    public Hashtag Hashtag { get; set; }
}
