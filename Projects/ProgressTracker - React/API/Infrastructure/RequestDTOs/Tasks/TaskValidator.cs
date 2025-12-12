using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Tasks;

public class TaskValidator : AbstractValidator<TaskRequest>
{
    public TaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(50).WithMessage("Task title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Task description must not exceed 500 characters.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
    }
}
