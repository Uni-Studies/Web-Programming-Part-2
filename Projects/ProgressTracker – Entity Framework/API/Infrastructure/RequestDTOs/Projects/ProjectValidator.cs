using System;
using System.Data;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Projects;

public class ProjectValidator : AbstractValidator<ProjectRequest>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Project name is required.")
        .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
        .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters.");   
    }
}
