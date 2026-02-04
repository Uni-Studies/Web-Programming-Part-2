using System;

namespace Common.Entities.ManyToManyEntities;

public class UserInterest
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int InterestId { get; set; }
    public virtual Interest Interest { get; set; }
}
