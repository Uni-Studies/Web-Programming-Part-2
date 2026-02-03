using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Common.Entities;
using Common.Entities.ManyToManyEntities;
using Common.Entities.UserSubmissionsEntities;
using Common.Enums;

namespace Common.Services;

public class SkillServices : BaseServices<Skill>
{
    public Skill GetByName(string name)
    {
        return Items.FirstOrDefault(x => x.Name.Equals(name));
    }

    public bool SkillExists(string name)
    {
        return Items.Any(x => x.Name == name);
    }
    public UserSkill AddSkillToSubmission(int userId, int skillId, UserSubmissionBaseEntity submission)
    {
        UserSkillServices userSkillServices = new UserSkillServices();
        var actualType = submission.GetType().BaseType ?? submission.GetType();
        SubmissionType typeEnum = Enum.Parse<SubmissionType>(actualType.Name);

        UserSkill userSkill = new UserSkill() { UserId = userId, SkillId = skillId, SubmissionId = submission.Id, SubmissionType = typeEnum};
        userSkillServices.Add(userSkill);

        return userSkill;
    }

    public void MapUserSkillToUserAndSkill(User user, Skill skill, UserSkill userSkill)
    {
       userSkill.User = user;
       userSkill.Skill = skill;
       Context.SaveChanges();
    }

}
