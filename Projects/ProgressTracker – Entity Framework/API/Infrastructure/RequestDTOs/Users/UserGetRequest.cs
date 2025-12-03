using System;
using API.Infrastructure.RequestDTOs.Shared;

namespace API.Infrastructure.RequestDTOs.Users;

public class UserGetRequest: BaseGetRequest<UsersGetFilterRequest>
{
    // public PagerRequest Pager {get; set;}
    // public string OrderBy {get; set;}
    // public bool SortAsc {get; set;}

    // public UsersGetFilterRequest Filter {get; set;}
}
