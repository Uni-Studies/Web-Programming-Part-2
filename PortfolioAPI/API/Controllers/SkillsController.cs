using System;
using System.Collections.Generic;
using API.Infrastructure.RequestDTOs.Skill;
using API.Infrastructure.ResponseDTOs.Skill;
using API.Services;
using Common;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : BaseCRUDController<Skill, SkillServices, SkillRequest, SkillsGetRequest, SkillsGetResponse>
    {
      
    }
}
