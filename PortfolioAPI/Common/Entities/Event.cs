using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Entities;

public class Event : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }

    public decimal Price { get; set; }
    public DateTime DeadlineDate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [JsonIgnore]
    public List<User> EnrolledUsers { get; set; }

}
