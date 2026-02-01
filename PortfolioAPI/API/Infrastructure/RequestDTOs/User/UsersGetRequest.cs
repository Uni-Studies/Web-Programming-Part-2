using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.User;

public class UsersGetRequest : BaseGetRequest
{
    public UsersGetFilterRequest Filter { get; set; }
}
