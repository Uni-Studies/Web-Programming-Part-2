using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Common.Entities.ManyToManyEntities;
using Common.Enums;
using Common.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UserSkillServices
{
    protected DbContext Context { get; set; } 
    protected DbSet<UserSkill> Items { get; set; }

    public UserSkillServices()
    {
        Context = new AppDbContext();
        Items = Context.Set<UserSkill>();
    }
    public void Add(UserSkill item)
    {
        if(Items.Contains(item))
            throw new Exception("Skill has been already added");
        Items.Add(item);
        Context.SaveChanges();
    }

    public void Delete(UserSkill item)
    {
        Items.Remove(item);
        Context.SaveChanges();
    }

    public UserSkill GetById(int userId, int skillId, SubmissionType submissionType, int submissionId)
    {
        return Items.FirstOrDefault(x => x.UserId == userId && 
                                x.SkillId == skillId && 
                                x.SubmissionType == submissionType && 
                                x.SubmissionId == submissionId);

    }

    public List<UserSkill> GetAll(Expression<Func<UserSkill, bool>> filter = null)
    {
        return Items.Where(filter).ToList();
    }

}
