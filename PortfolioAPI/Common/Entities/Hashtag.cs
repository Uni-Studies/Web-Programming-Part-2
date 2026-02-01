using System;
using System.Collections.Generic;

namespace Common.Entities;

public class Hashtag : BaseEntity
{
    public string Tag { get; set; }

    public virtual List<Post> Posts { get; set; }
}
