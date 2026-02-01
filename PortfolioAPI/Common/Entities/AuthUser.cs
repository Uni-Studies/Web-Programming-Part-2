using System;

namespace Common.Entities;

public class AuthUser : BaseEntity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public User User { get; set; }
}
