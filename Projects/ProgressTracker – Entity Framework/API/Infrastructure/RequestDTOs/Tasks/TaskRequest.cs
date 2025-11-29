using System;

namespace API.Infrastructure.RequestDTOs.Tasks;

public class TaskRequest
{
    public int ProjectId { get; set; }
    public int AssigneeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
