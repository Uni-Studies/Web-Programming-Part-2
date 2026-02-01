using System;
using API.Infrastructure.RequestDTOs.Course;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Course;

public class CoursesGetResponse : BaseGetResponse<Common.Entities.Course>
{
    public CoursesGetFilterRequest Filter { get; set; }
}
