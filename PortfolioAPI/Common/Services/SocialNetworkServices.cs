using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;

namespace Common.Services;

public class SocialNetworkServices : BaseServices<SocialNetwork>
{
    public bool AccountExists(string account)
    {
        return Items.Any(x => x.Account.Equals(account));
    }
}
