using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Education;

public class EducationRequest : SubmissionRequest
{
    public string Type { get; set; }
    public string Specialty { get; set; }
    public string School { get; set; }
    public string Location { get; set; }
}
