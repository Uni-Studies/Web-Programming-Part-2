using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogsGetRequest : BaseGetRequest
{
    public WorkLogsGetFilterRequest Filter { get; set; }
}
