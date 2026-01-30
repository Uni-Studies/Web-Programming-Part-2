using System;
using Common.Entities.UserDetailsEntities;

namespace Common.Entities;

public class Work : UserDetailsBaseEntity
{
    public string Sphere { get; set; }
    public string Occupation { get; set; }
    public decimal Salary { get; set; }
    public string Location { get; set; }
}
