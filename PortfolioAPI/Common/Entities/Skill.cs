using System;

namespace Common.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; }
    public int Importance { get; set; }
}
