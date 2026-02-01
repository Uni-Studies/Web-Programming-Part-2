using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Work;

public class WorkGetRequest : BaseGetRequest
{
    public WorkGetFilterRequest Filter { get; set; }
}
