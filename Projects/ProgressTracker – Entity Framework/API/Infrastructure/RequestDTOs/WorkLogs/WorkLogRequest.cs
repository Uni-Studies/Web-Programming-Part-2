using System;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogRequest
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public int WorkDuration {get; set;}
    public DateTime LogDate { get; set; }
}
