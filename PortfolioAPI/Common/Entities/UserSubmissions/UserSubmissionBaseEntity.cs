using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Entities.UserSubmissionsEntities;

public class UserSubmissionBaseEntity : BaseEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public StatusEnum CompletionStatus { get; set; }

    public virtual List<User> Users { get; set; }
}
