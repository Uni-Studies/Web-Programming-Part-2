using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;

public class UserServices : BaseServices<User>
{
    public User GetFullUser(int id, Expression<Func<User, bool>> filter = null)
    {
        var query = Items
            .Include(u => u.AuthUser)
            .AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return query.FirstOrDefault(u => u.Id == id);
    }

}

