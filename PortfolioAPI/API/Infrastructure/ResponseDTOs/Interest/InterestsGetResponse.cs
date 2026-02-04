using System;
using API.Infrastructure.RequestDTOs.Interest;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Interest;

public class InterestsGetResponse : BaseGetResponse<Common.Entities.Interest>
{
    public InterestsGetFilterRequest Filter { get; set; }
}
