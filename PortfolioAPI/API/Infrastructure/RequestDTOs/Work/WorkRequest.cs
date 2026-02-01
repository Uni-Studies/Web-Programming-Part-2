using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Work;

public class WorkRequest : SubmissionRequest
{
    public string Sphere { get; set; }
    public string Occupation { get; set; }
    public decimal Salary { get; set; }
    public string Location { get; set; }
    public string Company { get; set; }
}
