using System;
using Common.Entities.UserSubmissionsEntities;
using Common.Enums;

namespace Common.Entities.ManyToManyEntities;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }

    public SubmissionType SubmissionType { get; set; }
    public int SubmissionId { get; set; }

    public virtual User User { get; set; }
    public virtual Skill Skill { get; set; }
}
