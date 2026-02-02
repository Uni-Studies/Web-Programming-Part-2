using System;
using System.Collections.Generic;
using Common.Entities;

namespace API.Infrastructure.RequestDTOs.User;

public class UserRequest 
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Sex { get; set; }
    public DateOnly BirthDate { get; set; }
    public string BirthCity { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public string Nationality { get; set; }
    public string Details { get; set; }
    public string ProfilePicture { get; set; }
}
