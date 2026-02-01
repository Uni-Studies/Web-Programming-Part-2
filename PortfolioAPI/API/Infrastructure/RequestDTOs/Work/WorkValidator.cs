
using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Work;

public class WorkValidator : AbstractValidator<WorkRequest>
{
	public WorkValidator()
	{
        RuleFor(x => x.UserId)
			.GreaterThan(0).WithMessage("UserId must be a positive integer.");

		RuleFor(x => x.StartDate)
			.LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be on or before EndDate.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be on or after StartDate.");

		// Work-specific validations
		RuleFor(x => x.Sphere)
			.NotEmpty().WithMessage("Sphere is required.")
			.MaximumLength(150).WithMessage("Sphere must be at most 150 characters.");

		RuleFor(x => x.Occupation)
			.NotEmpty().WithMessage("Occupation is required.")
			.MaximumLength(150).WithMessage("Occupation must be at most 150 characters.");

		RuleFor(x => x.Salary)
			.GreaterThanOrEqualTo(0).WithMessage("Salary cannot be negative.");

		RuleFor(x => x.Location)
			.MaximumLength(200).WithMessage("Location must be at most 200 characters.");

        RuleFor(x => x.Company)
            .MaximumLength(200).WithMessage("Company must be at most 200 characters.");
	}
}
