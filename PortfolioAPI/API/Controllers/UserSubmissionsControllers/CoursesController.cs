using API.Infrastructure.RequestDTOs.Course;
using API.Infrastructure.ResponseDTOs.Course;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : BaseCRUDController<Course, CourseServices, CourseRequest, CoursesGetRequest, CoursesGetResponse>
    {
    }
}
