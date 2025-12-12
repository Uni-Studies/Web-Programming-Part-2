using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Users;

public class UsersGetRequest : BaseGetRequest
{
    public UsersGetFilterRequest Filter { get; set; }
}
