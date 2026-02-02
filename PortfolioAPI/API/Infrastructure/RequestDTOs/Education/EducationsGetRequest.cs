using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Education;

public class EducationsGetRequest : BaseGetRequest
{
    public EducationsGetFilterRequest Filter { get; set; }
}
