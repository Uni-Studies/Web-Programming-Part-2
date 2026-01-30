using System;
using Common.Entities.UserSubmissionsEntities;

namespace Common.Entities.UserDetailsEntities;

public class UserSubmission
{
    public int UserId { get; set; }
    public int SubmissionId { get; set; }
    public User User { get; set; }
    public UserSubmissionBaseEntity Submission { get; set; }
}
