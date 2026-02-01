using System;

namespace API.Infrastructure.RequestDTOs.Auth;

public class AuthTokenRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
