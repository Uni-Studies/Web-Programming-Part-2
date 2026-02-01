using System;
using System.Collections.Generic;
using Common.Entities.UserSubmissionsEntities;


namespace Common.Entities;

public class Work : UserSubmissionBaseEntity
{
    public string Sphere { get; set; }
    public string Occupation { get; set; }
    public decimal Salary { get; set; }
    public string Location { get; set; }
    public string Company { get; set; }
}
