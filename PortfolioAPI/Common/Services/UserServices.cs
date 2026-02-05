using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Common.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UserServices : BaseServices<User>
{
    public FullUser GetFullUser(int id, Expression<Func<User, bool>> filter = null)
    {
        var user = GetById(id);
        
        AuthUserServices authUserServices = new AuthUserServices();
        string email = authUserServices.GetEmail(id);
        string username = authUserServices.GetUsername(id);

        FullUser fullUser = new FullUser
        {
            Email = email,
            Username = username,
            User = user
        };

        return fullUser;
    }

}

