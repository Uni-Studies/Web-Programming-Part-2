using System;

namespace Common.Entities;

public class Image : BaseEntity
{
    public int ItemId { get; set; }
    public string Imagepath { get; set; }
}
