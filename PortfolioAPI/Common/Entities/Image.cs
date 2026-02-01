using System;

namespace Common.Entities;

public class Image : BaseEntity
{
    public int PostId { get; set; }
    public string Imagepath { get; set; }

    public virtual Post Post { get; set; }
}
