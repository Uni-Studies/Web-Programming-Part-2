using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Interest;

public class InterestsGetRequest : BaseGetRequest
{
    public InterestsGetFilterRequest Filter { get; set; }
}
