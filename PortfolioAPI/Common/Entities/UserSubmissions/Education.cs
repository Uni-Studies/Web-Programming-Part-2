using System;
using Common.Entities.UserDetailsEntities;

namespace Common.Entities;

public class Education : UserDetailsBaseEntity
{
    public string Type { get; set; }
    public string Specialty { get; set; }
    public string School { get; set; }
    public string Location { get; set; }
}
