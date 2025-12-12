using System;
using API.Infrastructure.RequestDTOs.Shared;
using API.Infrastructure.RequestDTOs.Users;
using API.Infrastructure.ResponseDTOs.Shared;
using Common.Entities;

namespace API.Infrastructure.ResponseDTOs.Users;

public class UsersGetResponse : BaseGetResponse<User>
{
    public UsersGetFilterRequest Filter { get; set; }
}
