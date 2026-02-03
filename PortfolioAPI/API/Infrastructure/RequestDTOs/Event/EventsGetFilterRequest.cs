using System;

namespace API.Infrastructure.RequestDTOs.Event;

public class EventsGetFilterRequest
{
    public string Title { get; set; }
    public string Location { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

}
