using System;
using Common.Entities;

namespace Common.Services;

public class ProjectServices : BaseServices<Project>
{
    public Project GetProjectByUserId(int userId, int projectId)
    {
        var project = GetById(projectId);
        if(project is null)
            throw new ArgumentException("Project not found!");

        if(project.UserId != userId)
            throw new ArgumentException("Project has another user!"); 
               
        return project;
    }
}
