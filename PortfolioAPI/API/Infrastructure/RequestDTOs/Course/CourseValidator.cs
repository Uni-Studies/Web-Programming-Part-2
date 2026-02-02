
using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Course;

public class CourseValidator : AbstractValidator<CourseRequest>
{
	public CourseValidator()
	{
		// Submission base validations

		RuleFor(x => x.StartDate)
			.LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be on or before EndDate.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be on or after StartDate.");

		// Course-specific validations
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MaximumLength(300).WithMessage("Name must be at most 300 characters.");

		RuleFor(x => x.Price)
			.GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

		RuleFor(x => x.Tutor)
			.MaximumLength(200).WithMessage("Tutor must be at most 200 characters.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description is required.")
			.MaximumLength(5000).WithMessage("Description must be at most 5000 characters.");

		RuleFor(x => x.Place)
			.MaximumLength(200).WithMessage("Place must be at most 200 characters.");

		RuleFor(x => x.Language)
			.MaximumLength(100).WithMessage("Language must be at most 100 characters.");
	}
}
