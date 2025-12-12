using System;
using API.Infrastructure.RequestDTOs.Projects;
using API.Infrastructure.ResponseDTOs.Shared;
using Common.Entities;

namespace API.Infrastructure.ResponseDTOs.Projects;

public class ProjectsGetResponse : BaseGetResponse<Project>
{
    public ProjectsGetFilterRequest Filter { get; set; }
}