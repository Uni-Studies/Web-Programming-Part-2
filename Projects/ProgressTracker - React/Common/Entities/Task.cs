using System;
using System.ComponentModel;

namespace Common.Entities;

public class Task : BaseEntity
{
    public int OwnerId { get; set; }
    public int ProjectId { get; set; }
    public int AssigneeId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }

    public virtual User Owner { get; set; }
    public virtual Project Project { get; set; }
    public virtual User Assignee { get; set; }
}
