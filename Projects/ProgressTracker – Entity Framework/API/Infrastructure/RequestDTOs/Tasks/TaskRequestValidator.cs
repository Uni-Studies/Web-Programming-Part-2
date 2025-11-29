using FluentValidation;
using API.Infrastructure.RequestDTOs.Tasks;
using System;

namespace API.Infrastructure.Validators.Tasks;

public class TaskValidator : AbstractValidator<TaskRequest>
{
    public TaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.DueDate)
            .Must(d => d == default || d > DateTime.UtcNow.AddMinutes(-1))
            .WithMessage("DueDate must be in the future if specified.");

        // never DateTime.Now because of time zones
        // use 0-hour zone so use UTC
    }
}