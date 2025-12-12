using System;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogsGetFilterRequest
{
    public int? UserId { get; set; }
    public int? TaskId { get; set; }

    public DateTime? TimestampFrom { get; set; }
    public DateTime? TimestampTo { get; set; }
}
