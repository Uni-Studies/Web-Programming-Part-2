using System;
using Common.Entities;

namespace Common.Services;

public class EducationServices : BaseServices<Education>
{
    public Education GetEducationByUserId(int userId, int educationId)
    {
        var education = GetById(educationId);
        if(education is null)
            throw new ArgumentException("Education not found!");

        if(education.UserId != userId)
            throw new ArgumentException("Education has another user!"); 
               
        return education;
        
    }
}
