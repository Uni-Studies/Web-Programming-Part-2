using System;
using System.Diagnostics.Eventing.Reader;

namespace Common.Entities.ManyToManyEntities;

public class EventUser
{
    public int EventId { get; set; }
    public virtual Event Event { get; set; }

    public bool IsOwner { get; set; }
    public virtual int UserId { get; set; }
    public virtual User User { get; set; }
}
