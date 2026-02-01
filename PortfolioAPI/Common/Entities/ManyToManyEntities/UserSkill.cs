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

    public User User { get; set; }
    public Skill Skill { get; set; }
}
