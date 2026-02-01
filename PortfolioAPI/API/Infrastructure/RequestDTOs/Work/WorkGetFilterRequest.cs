using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Work;

public class WorkGetFilterRequest : WorkRequest
{
    public decimal? SalaryFrom { get; set; }
    public decimal? SalaryTo { get; set; }
}
