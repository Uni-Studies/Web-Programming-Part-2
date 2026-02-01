using System;
using System.Collections.Generic;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities;

public class Course : UserSubmissionBaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Tutor { get; set; }
    public string Description { get; set; }
    public string Place { get; set; }
    public string Language { get; set; }
    public bool HasCertificate { get; set; }
}
