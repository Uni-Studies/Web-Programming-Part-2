using System;

namespace API.Infrastructure.RequestDTOs.Users;

public class UsersGetFilterRequest
{
    // int or date - put ?
    public string Username {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
}
