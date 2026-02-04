using System;

namespace Common.Entities.DTOs;

public class FullUser
{
    public string Username { get; set; }
    public string Email { get; set; }
    public User User { get; set; }

}
