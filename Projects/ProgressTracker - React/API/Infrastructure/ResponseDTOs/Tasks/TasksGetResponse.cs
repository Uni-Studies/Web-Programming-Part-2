using System;
using System.Threading.Tasks;
using API.Infrastructure.RequestDTOs.Tasks;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Tasks;

public class TasksGetResponse : BaseGetResponse<Task>
{
    public TasksGetFilterRequest Filter { get; set; }
}