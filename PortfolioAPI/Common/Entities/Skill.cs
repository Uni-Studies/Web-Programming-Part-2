using System;
using System.Collections.Generic;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; }
    public int Importance { get; set; }
    public virtual List<User> Users { get; set; }
    public virtual List<UserSubmissionBaseEntity> UserSubmissions { get; set; }
}
