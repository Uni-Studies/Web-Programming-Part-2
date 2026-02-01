using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Project;

public class ProjectValidation : AbstractValidator<ProjectRequest>
{
	public ProjectValidation()
	{
		RuleFor(x => x.StartDate)
			.LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be on or before EndDate.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be on or after StartDate.");
		
        RuleFor(x => x.Type)
			.NotEmpty().WithMessage("Type is required.")
			.MaximumLength(50).WithMessage("Type must be at most 50 characters.");

		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(200).WithMessage("Title must be at most 200 characters.");

		RuleFor(x => x.Topic)
			.MaximumLength(200).WithMessage("Topic must be at most 200 characters.");

		RuleFor(x => x.Mentor)
			.MaximumLength(150).WithMessage("Mentor must be at most 150 characters.");

		RuleFor(x => x.PagesCount)
			.GreaterThanOrEqualTo(0).WithMessage("PagesCount cannot be negative.");

		RuleFor(x => x.Language)
			.MaximumLength(100).WithMessage("Language must be at most 100 characters.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description is required.")
			.MaximumLength(5000).WithMessage("Description must be at most 5000 characters.");

		RuleFor(x => x.Link)
			.MaximumLength(1000).WithMessage("Link must be at most 1000 characters.")
			.Must(link => string.IsNullOrWhiteSpace(link) || Uri.IsWellFormedUriString(link, UriKind.Absolute))
			.WithMessage("Link must be a valid absolute URL when provided.");
	}
}
