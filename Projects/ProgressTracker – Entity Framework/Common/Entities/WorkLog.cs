using System;

namespace Common.Entities;

public class WorkLog : BaseEntity
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public int WorkDuration {get; set;}
    public DateTime LogDate { get; set; }   

    public virtual User User { get; set; }
    public virtual Task Task { get; set; }
}
