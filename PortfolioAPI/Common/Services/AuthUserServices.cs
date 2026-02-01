using System;
using System.Linq;
using Common.Entities;

namespace Common.Services;

public class AuthUserServices : BaseServices<AuthUser>
{
    public AuthUser Authenticate(string username, string password)
    {
        return Items
            .FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password
            );
    }   

    public AuthUser Register(string username, string email, string password)
    {

        if (Items.Any(u => u.Username == username))
            throw new Exception("Username already exists");

        if (Items.Any(u => u.Email == email))
            throw new Exception("Email already exists");

        var authUser = new AuthUser
        {
            Username = username,
            Email = email,
            Password = password
        };

        authUser.User = new User();

        Save(authUser);
         
/*         var user = new User
        {
            Id = authUser.Id,
        };

        UserServices userService = new UserServices();
        userService.Save(user); */

        return authUser;
    }

    public string GetUsername(int userId)
    {
        var user = GetById(userId);
        return user.Username;
    }

    public string GetEmail(int userId)
    {
        var user = GetById(userId);
        return user.Email;
    }
}
