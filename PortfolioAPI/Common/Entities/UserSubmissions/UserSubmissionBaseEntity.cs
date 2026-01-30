using System;
using System.Collections.Generic;

namespace Common.Entities.UserSubmissionsEntities;

public class UserSubmissionBaseEntity : BaseEntity
{
    public virtual List<User> Users { get; set; }
    public virtual List<Skill> Skills { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
