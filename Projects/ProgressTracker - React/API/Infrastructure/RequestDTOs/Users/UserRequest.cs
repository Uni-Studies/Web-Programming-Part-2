using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Infrastructure.RequestDTOs.Projects;

namespace API.Infrastructure.RequestDTOs.Users;

public class UserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
