using System;
using API.Infrastructure.RequestDTOs.Event;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Event;

public class EventsGetResponse : BaseGetResponse<Common.Entities.Event>
{
    public EventsGetFilterRequest Filter { get; set; }
}
