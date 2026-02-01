using System;

namespace Common.Entities;

public class AuthUser : BaseEntity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public virtual User User { get; set; }
}
