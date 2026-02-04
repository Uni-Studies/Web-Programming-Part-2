using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;
using Common.Entities.ManyToManyEntities;

namespace Common.Services;

public class InterestServices : BaseServices<Interest>
{
    public Interest GetByName(string name)
    {
        return Items.FirstOrDefault(x => x.Name.Equals(name));
    }

    public bool InterestExists(string name)
    {
        return Items.Any(x => x.Name == name);
    }

    public bool UserHasInterest(User user, string interestName)
    {
        Context.Attach(user);
        return user.Interests.Any(i => i.Name.Equals(interestName));
    }
    public void AddInterestToUser(string interestName, User user)
    {
        Context.Add(user);

        var interest = GetByName(interestName);
        user.Interests.Add(interest);

        Context.SaveChanges();
    }

    public void RemoveInterestFromUser(string interestName, User user)
    {
        Context.Add(user);

        var interest = GetByName(interestName);
        user.Interests.Add(interest);

        Context.SaveChanges();
    }

    public List<Interest> GetUserInterests(User user)
    {
        if(user is null)
            throw new Exception("User is not found");
        
        Context.Attach(user); 
        return user.Interests;
    }
}
