using System;
using API.Infrastructure.RequestDTOs.User;
using API.Infrastructure.ResponseDTOs.Shared;

namespace API.Infrastructure.ResponseDTOs.User;

public class UsersGetResponse : BaseGetResponse<Common.Entities.User>
{
    public UsersGetFilterRequest Filter { get; set; }
}
