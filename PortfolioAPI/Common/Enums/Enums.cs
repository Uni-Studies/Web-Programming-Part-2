using System;

namespace Common.Enums;

public enum StatusEnum
{
    Planned,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}

public enum PostPrivacyLevel
{
    Public,
    Private
}

public enum SubmissionType
{
    Project = 1,
    Course = 2,
    Work = 3,
    Education = 4
}
