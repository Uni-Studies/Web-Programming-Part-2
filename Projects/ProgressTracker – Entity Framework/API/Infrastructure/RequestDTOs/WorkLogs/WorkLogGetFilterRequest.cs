using System;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogGetFilterRequest
{
    public int? UserId { get; set; }
    public int? TaskId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public int MinutesDuration { get; set; }
}
