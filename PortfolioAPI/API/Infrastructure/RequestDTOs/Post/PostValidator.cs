using System;
using System.Linq;
using FluentValidation;
using Common.Entities;

namespace API.Infrastructure.RequestDTOs.Post;

public class PostValidator : AbstractValidator<PostRequest>
{
	public PostValidator()
	{
		RuleFor(x => x.UserId)
			.GreaterThan(0).WithMessage("UserId must be a positive integer.");

		RuleFor(x => x.Location)
			.MaximumLength(200).WithMessage("Location must be at most 200 characters.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description is required.")
			.MaximumLength(2000).WithMessage("Description must be at most 2000 characters.");

		RuleFor(x => x.CreatedAt)
			.LessThanOrEqualTo(DateTime.UtcNow.AddYears(5)).WithMessage("CreatedAt cannot be in the far future.");

		//RuleFor(x => x.LikesCount)
			//.GreaterThanOrEqualTo(0).WithMessage("LikesCount cannot be negative.");

		RuleFor(x => x.Images)
			.NotNull().WithMessage("Images list must be provided (can be empty).")
			.Must(list => list.Count <= 10).WithMessage("A post can have at most 10 images.");

		RuleFor(x => x.Hashtags)
			.NotNull().WithMessage("Hashtags list must be provided (can be empty).")
			.Must(list => list.Count <= 20).WithMessage("A post can have at most 20 hashtags.");
        
	}
}
