using System;

namespace API.Infrastructure.RequestDTOs.Projects;

public class ProjectsGetFilterRequest
{
    public int? OwnerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
