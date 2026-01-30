using System;
using Common.Entities.UserDetailsEntities;

namespace Common.Entities;

public class Course : UserDetailsBaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Tutor { get; set; }
    public string Description { get; set; }
    public string Place { get; set; }
    public string Language { get; set; }
    public bool HasCertificate { get; set; }
}
