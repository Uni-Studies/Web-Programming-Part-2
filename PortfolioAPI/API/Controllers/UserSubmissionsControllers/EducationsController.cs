using API.Infrastructure.RequestDTOs.Education;
using API.Infrastructure.ResponseDTOs.Education;
using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserSubmissionsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : BaseCRUDController<Education, EducationServices, EducationRequest, EducationsGetRequest, EducationsGetResponse>
    {
    }
}
