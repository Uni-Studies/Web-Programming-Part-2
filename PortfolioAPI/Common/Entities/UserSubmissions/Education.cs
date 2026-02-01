using System;
using System.Collections.Generic;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class Education : UserSubmissionBaseEntity
{
    public string Type { get; set; }
    public string Specialty { get; set; }
    public string School { get; set; }
    public string Location { get; set; }
}
