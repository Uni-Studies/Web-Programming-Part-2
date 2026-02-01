using System;
using System.Collections.Generic;
using Common.Entities;
using Common.Enums;

namespace API.Infrastructure.RequestDTOs.Shared;

public class SubmissionRequest
{
    public int UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public StatusEnum CompletionStatus { get; set; } = StatusEnum.Planned;

   /*  public virtual User User { get; set;}
    public virtual List<Skill> Skills { get; set; } */
}
