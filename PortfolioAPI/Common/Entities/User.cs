using System;

namespace Common.Entities;

public class User : AuthUser
{
    public string Sex { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthCity { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public string Nationality { get; set; }
    public string Details { get; set; }
    public string Image { get; set; }
}
