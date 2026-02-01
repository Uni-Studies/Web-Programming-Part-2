
using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Education;

public class EducationValidator : AbstractValidator<EducationRequest>
{
	public EducationValidator()
	{
		// Submission base validations
		RuleFor(x => x.UserId)
			.GreaterThan(0).WithMessage("UserId must be a positive integer.");

		RuleFor(x => x.StartDate)
			.LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be on or before EndDate.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be on or after StartDate.");

		// Education-specific validations
		RuleFor(x => x.Type)
			.NotEmpty().WithMessage("Type is required.")
			.MaximumLength(100).WithMessage("Type must be at most 100 characters.");

		RuleFor(x => x.Specialty)
			.NotEmpty().WithMessage("Specialty is required.")
			.MaximumLength(200).WithMessage("Specialty must be at most 200 characters.");

		RuleFor(x => x.School)
			.NotEmpty().WithMessage("School is required.")
			.MaximumLength(250).WithMessage("School must be at most 250 characters.");

		RuleFor(x => x.Location)
			.MaximumLength(200).WithMessage("Location must be at most 200 characters.");
	}
}
