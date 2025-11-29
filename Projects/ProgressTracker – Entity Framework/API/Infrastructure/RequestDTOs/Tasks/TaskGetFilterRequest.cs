using System;

namespace API.Infrastructure.RequestDTOs.Tasks;

public class TaskGetFilterRequest
{
    public int? ProjectId { get; set; }
    public int? AssigneeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
}
