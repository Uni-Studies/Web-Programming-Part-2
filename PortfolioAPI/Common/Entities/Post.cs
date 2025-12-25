using System;

namespace Common.Entities;

public class Post : BaseEntity
{
    public int ID { get; private set; }
    public int UserId { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
}
