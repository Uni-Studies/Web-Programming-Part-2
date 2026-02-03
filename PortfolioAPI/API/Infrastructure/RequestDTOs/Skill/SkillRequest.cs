using System;

namespace API.Infrastructure.RequestDTOs.Skill;

public class SkillRequest
{
    public string Name { get; set; }
    public int Importance { get; set; }
}
