using System;
using API.Infrastructure.RequestDTOs.Users;
using API.Infrastructure.ResponseDTOs.Shared;
using Common.Entities;

namespace API.Infrastructure.ResponseDTOs.Users;

public class UsersGetResponse : BaseGetResponse<User, UsersGetFilterRequest>
{

}
