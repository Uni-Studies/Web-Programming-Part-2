using System;
using System.Text.Json.Serialization;

namespace Common.Entities;

public class Image : BaseEntity
{
    public int PostId { get; set; }
    public string ImagePath { get; set; }

    [JsonIgnore]
    public virtual Post Post { get; set; }
}
