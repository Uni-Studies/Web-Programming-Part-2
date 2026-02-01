using System;
using API.Infrastructure.RequestDTOs.Work;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Work;

public class WorkGetResponse : BaseGetResponse<Common.Entities.Work>
{
    public WorkGetFilterRequest Filter { get; set; }
}
