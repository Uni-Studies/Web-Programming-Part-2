using System;
using System.Collections.Generic;
using Common.Entities.ManyToManyEntities;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; }
    public virtual List<UserSkill> UserSkills { get; set; }
}
