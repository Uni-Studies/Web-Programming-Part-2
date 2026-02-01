using System;
using API.Infrastructure.RequestDTOs.Shared;
using Common.Enums;

namespace API.Infrastructure.RequestDTOs.Project;

public class ProjectsGetFilterRequest : SubmissionRequest
{
    public string Type { get; set; }
    public string Title { get; set; }
    public string Topic { get; set; }
    public string Mentor { get; set; }
    public int PagesCount { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
   
}
