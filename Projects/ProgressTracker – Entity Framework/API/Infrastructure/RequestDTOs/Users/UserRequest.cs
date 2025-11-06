using System;
using System.ComponentModel.DataAnnotations;

namespace API.Infrastructure.RequestDTOs.Users;

public class UserRequest
{
    //[Required(ErrorMessage = "This field is required.")]
    //[MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    public string Username { get; set; }

    //[Required(ErrorMessage = "This field is required.")]
    //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    //[Required(ErrorMessage = "This field is required.")]
    public string FirstName { get; set; }   

    //[Required(ErrorMessage = "This field is required.")]
    public string LastName { get; set; }
}
