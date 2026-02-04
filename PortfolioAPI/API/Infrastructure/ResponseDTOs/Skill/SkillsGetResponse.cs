using System;
using System.Collections.Generic;
using API.Infrastructure.RequestDTOs.Skill;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.Skill;

public class SkillsGetResponse : BaseGetResponse<Common.Entities.Skill>
{
    public SkillsGetFilterRequest Filter { get; set; }

    public Dictionary<string, double> SkillImportance { get; set; }
}
