using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Course;

public class CoursesGetRequest : BaseGetRequest
{
    public CoursesGetFilterRequest Filter { get; set; }
}
