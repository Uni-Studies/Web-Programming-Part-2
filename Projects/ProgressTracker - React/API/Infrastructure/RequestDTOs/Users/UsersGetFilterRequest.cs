using System;

namespace API.Infrastructure.RequestDTOs.Users;

public class UsersGetFilterRequest
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
