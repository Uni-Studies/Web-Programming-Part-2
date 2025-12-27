using System;
using Common.Entities.UserDetailsEntities;
using Microsoft.Identity.Client;

namespace Common.Entities.ManyToManyEntities;

public class SubmissionSkill
{
    public int SubmissionId { get; set; }
    public int SkillId { get; set; }
    public UserDetailsBaseEntity Submission { get; set; }
    public Skill Skill { get; set; }
}
