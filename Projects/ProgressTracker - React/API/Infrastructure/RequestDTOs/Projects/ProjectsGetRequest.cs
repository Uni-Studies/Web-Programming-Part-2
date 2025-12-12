using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Projects;

public class ProjectsGetRequest : BaseGetRequest
{
    public ProjectsGetFilterRequest Filter { get; set; }
}
