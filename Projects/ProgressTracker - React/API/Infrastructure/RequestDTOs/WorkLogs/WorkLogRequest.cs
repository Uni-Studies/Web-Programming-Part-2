using System;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogRequest
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public DateTime Timestamp { get; set; }
}
