using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Hashtag;

public class HashtagValidator : AbstractValidator<HashtagRequest>
{
	public HashtagValidator()
	{
		RuleFor(x => x.Tag)
			.NotEmpty().WithMessage("Tag is required.")
			.MaximumLength(100).WithMessage("Tag must be at most 100 characters.")
			.Matches("^#?[A-Za-z0-9_]+$").WithMessage("Tag can contain only letters, numbers and underscores, and may start with '#'.");
	}
}
