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
    public UserSkill AddSkillToSubmission(int userId, int skillId, UserSubmissionBaseEntity submission, int importance)
    {
        UserSkillServices userSkillServices = new UserSkillServices();
        var actualType = submission.GetType().BaseType ?? submission.GetType();
        SubmissionType typeEnum = Enum.Parse<SubmissionType>(actualType.Name);

        UserSkill userSkill = new UserSkill() { UserId = userId, SkillId = skillId, SubmissionId = submission.Id, SubmissionType = typeEnum, Importance = importance };
        userSkillServices.Add(userSkill);

        return userSkill;
    }

    public void MapUserSkillToUserAndSkill(User user, Skill skill, UserSkill userSkill)
    {
       userSkill.User = user;
       userSkill.Skill = skill;
       Context.SaveChanges();
    }

    public List<Skill> GetUserSkills(User user)
    {
        var skills = new List<Skill>();
        var userSkills = user.UserSkills;
        foreach(var userSkill in userSkills)
        {
            var skill = GetById(userSkill.SkillId);
            skills.Add(skill);
        }
        return skills;
    }

    /* public List<Skill> SkillsByImportance()
    {
        UserSkillServices userSkillServices = new UserSkillServices();
        
    } */

    /* public List<User> GetUsersBySkill(int skillId)
    {
        
    } */
}
