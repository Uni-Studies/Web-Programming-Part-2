using System;
using Common.Entities.UserSubmissionsEntities;
using Common.Enums;

namespace Common.Entities;

public class Project : UserSubmissionBaseEntity
{
    public string Type { get; set; }
    public string Title { get; set; }
    public string Topic { get; set; }
    public string Mentor { get; set; }
    public int PagesCount { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    
}
