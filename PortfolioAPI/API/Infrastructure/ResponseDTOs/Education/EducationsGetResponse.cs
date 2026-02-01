using System;
using API.Infrastructure.RequestDTOs.Education;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Education;

public class EducationsGetResponse : BaseGetResponse<Common.Entities.Education>
{
    public EducationsGetFilterRequest Filter { get; set; }
}
