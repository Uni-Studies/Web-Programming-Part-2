using System;
using FluentValidation;
using API.Infrastructure.RequestDTOs.WorkLogs;

namespace API.Infrastructure.RequestDTOs.WorkLogs;

public class WorkLogRequestValidator : AbstractValidator<WorkLogRequest>
{
    public WorkLogRequestValidator()
    {
        RuleFor(x => x.WorkDuration)
            .GreaterThan(0).WithMessage("WorkDuration must be a positive integer representing minutes.");

        _ = RuleFor(x => x.LogDate)
            .Must(d => d != default && d <= DateTime.UtcNow)
            .WithMessage("LogDate must be specified and cannot be in the future.");
    }
}
