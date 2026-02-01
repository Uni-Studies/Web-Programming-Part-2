using System;
using API.Infrastructure.RequestDTOs.Project;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Project;

public class ProjectsGetResponse : BaseGetResponse<Common.Entities.Project>
{
    public ProjectsGetFilterRequest Filter { get; set; }
}
