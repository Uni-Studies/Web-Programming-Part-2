using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Projects;

public class ProjectsGetFilterRequest
{
    public int? OwnerId {get; set;} //int? is the short form of Nullable<int>; default value is null
    public string Title {get; set;}
    public string Description {get; set;}

}
