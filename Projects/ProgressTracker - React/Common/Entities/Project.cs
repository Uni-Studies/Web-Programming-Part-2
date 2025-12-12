using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

public class Project : BaseEntity
{
    public int OwnerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public virtual User Owner { get; set; }
    public virtual List<User> Members { get; set; }
}
