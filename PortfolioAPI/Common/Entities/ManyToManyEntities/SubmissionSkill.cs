using System;
using Common.Entities.UserSubmissionsEntities;
using Microsoft.Identity.Client;

namespace Common.Entities.ManyToManyEntities;

public class SubmissionSkill
{
    public int SubmissionId { get; set; }
    public int SkillId { get; set; }
    public UserSubmissionBaseEntity Submission { get; set; }
    public Skill Skill { get; set; }
}
