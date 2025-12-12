using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Projects;

public class ProjectValidator : AbstractValidator<ProjectRequest>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Project title is required.")
            .MaximumLength(50).WithMessage("Project title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Project description must not exceed 500 characters.");
    }
}
