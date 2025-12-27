using System;

namespace Common.Entities.UserDetailsEntities;

public class UserSubmission
{
    public int UserId { get; set; }
    public int SubmissionId { get; set; }
    public User User { get; set; }
    public UserDetailsBaseEntity Submission { get; set; }
}
