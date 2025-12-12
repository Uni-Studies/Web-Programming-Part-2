using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Tasks;

public class TasksGetRequest : BaseGetRequest
{
    public TasksGetFilterRequest Filter { get; set; }
}
