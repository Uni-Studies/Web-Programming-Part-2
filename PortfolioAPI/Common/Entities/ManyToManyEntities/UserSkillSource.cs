using System;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities.ManyToManyEntities;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public int SourceId { get; set; }
    public User User { get; set; }
    public Skill Skill { get; set; }
    public UserSubmissionBaseEntity Source { get; set; }
}
