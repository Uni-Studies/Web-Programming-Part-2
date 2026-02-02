using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Common.Enums;

namespace Common.Entities.UserSubmissionsEntities;

public class UserSubmissionBaseEntity : BaseEntity
{
    public int UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public StatusEnum CompletionStatus { get; set; }

    public virtual User User { get; set; }
}
