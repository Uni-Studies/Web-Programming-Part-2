using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Entities;

public class Project
{
    public int Id { get; set; } // EF recognises this as the primary key by convention
    public int OwnerId { get; set; }    // the destination column is always the primary key of another table
    public string Title { get; set; }
    public string Description { get; set; }

    public virtual User Owner { get; set; } // navigation property; here the nav prop is good
}
