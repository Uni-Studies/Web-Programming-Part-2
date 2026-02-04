using System;
using System.Collections.Generic;

namespace API.Infrastructure.RequestDTOs.Skill;

public class SkillsGetFilterRequest 
{
    public string Name { get; set; }
    public int? Importance { get; set; } 
}
