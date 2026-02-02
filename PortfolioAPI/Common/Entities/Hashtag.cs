using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Entities;

public class Hashtag : BaseEntity
{
    public string Tag { get; set; }

    [JsonIgnore]
    public virtual List<Post> Posts { get; set; }
}
