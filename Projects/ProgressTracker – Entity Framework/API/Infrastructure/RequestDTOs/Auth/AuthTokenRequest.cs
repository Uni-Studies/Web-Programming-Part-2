using System;
using System.ComponentModel.DataAnnotations;

namespace API.Infrastructure.RequestDTOs.Auth;

public class AuthTokenRequest
{
    //[Required(ErrorMessage = "This field is required.")]
    //[MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
    public string Username { get; set; }

    //[Required(ErrorMessage = "This field is required.")]
    //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
}
