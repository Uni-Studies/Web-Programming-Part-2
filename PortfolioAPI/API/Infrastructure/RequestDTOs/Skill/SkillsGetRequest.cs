using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Skill;

public class SkillsGetRequest : BaseGetRequest
{
    public SkillsGetFilterRequest Filter { get; set; }
}
