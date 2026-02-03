using System;
using Common.Entities;

namespace Common.Services;

public class CourseServices : BaseServices<Course>
{
    public Course GetCourseByUserId(int userId, int courseId)
    {
        var course = GetById(courseId);
        if(course is null)
            throw new ArgumentException("Course not found!");

        if(course.UserId != userId)
            throw new ArgumentException("Course has another user!"); 
               
        return course;
    }
}
