using System;
using System.Collections.Generic;
using Common.Entities.UserDetailsEntities;

namespace Common.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; }
    public int Importance { get; set; }
    public List<User> Users { get; set; }
}
