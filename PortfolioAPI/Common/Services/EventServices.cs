using System;
using System.Collections.Generic;
using Common.Entities;

namespace Common.Services;

public class EventServices : BaseServices<Event>
{
   public void EnrollUser(User user, Event eventEntity)
    {
        //Context.Attach(user);
        //Context.Attach(post);

        if (user.Events.Contains(eventEntity))
        {
            throw new ArgumentException("Post has already been saved!");
        }
        
        user.Events.Add(eventEntity);
        Context.SaveChanges();   
    }

    public void CancelEnrollment(User user, Event eventEntity)
    {
        user.Events.Remove(eventEntity);
        Context.SaveChanges(); 
    }

    public List<Event> GetUserEvents(User user, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
    {
        return user.Events; 
    }

    public List<User> GetEventParticipants(int eventId)
    {
        Event searchedEvent = GetById(eventId);
        if(searchedEvent == null)
            throw new Exception("Event not found");
            
        return searchedEvent.EnrolledUsers;
    }
} 
