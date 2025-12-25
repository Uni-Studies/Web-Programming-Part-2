using System;

namespace Common.Entities;

public class Work : BaseEntity
{
    public string Sphere { get; set; }
    public string Occupation { get; set; }
    public DateOnly StartedAt { get; set; }
    public DateOnly ContinuedTo { get; set; }
    public string Location { get; set; }
    public decimal Salary { get; set; }
}
