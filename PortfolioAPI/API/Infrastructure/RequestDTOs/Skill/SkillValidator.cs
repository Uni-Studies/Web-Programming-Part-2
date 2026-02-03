using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Skill;

public class SkillValidator : AbstractValidator<SkillRequest>
{
	public SkillValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MaximumLength(200).WithMessage("Name must be at most 200 characters.");

		RuleFor(x => x.Importance)
			.GreaterThanOrEqualTo(1).WithMessage("Importance must be at least 1.")
			.LessThanOrEqualTo(10).WithMessage("Importance must be at most 10.");
	}
}
