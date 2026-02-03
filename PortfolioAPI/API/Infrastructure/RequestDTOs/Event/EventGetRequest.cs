using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Event;

public class EventGetRequest : BaseGetRequest
{
    public EventsGetFilterRequest Filter { get; set; }
}
