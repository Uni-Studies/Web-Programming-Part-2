using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Event;

public class EventValidator : AbstractValidator<EventRequest>
{
	public EventValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(300).WithMessage("Title must be at most 300 characters.");

		RuleFor(x => x.Description)
			.NotEmpty().WithMessage("Description is required.")
			.MaximumLength(5000).WithMessage("Description must be at most 5000 characters.");

		RuleFor(x => x.Location)
			.MaximumLength(200).WithMessage("Location must be at most 200 characters.");

		RuleFor(x => x.Type)
			.MaximumLength(100).WithMessage("Type must be at most 100 characters.");

		RuleFor(x => x.Capacity)
			.GreaterThanOrEqualTo(0).WithMessage("Capacity cannot be negative.");

		RuleFor(x => x.Price)
			.GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

		RuleFor(x => x.StartDate)
			.LessThanOrEqualTo(x => x.EndDate).WithMessage("StartDate must be on or before EndDate.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate).WithMessage("EndDate must be on or after StartDate.");

		RuleFor(x => x.StartTime)
			.LessThanOrEqualTo(x => x.EndTime).WithMessage("StartTime must be on or before EndTime.");

		RuleFor(x => x.DeadlineDate)
			.Must((req, deadline) => deadline <= req.StartDate.ToDateTime(req.StartTime))
			.WithMessage("DeadlineDate must be on or before the event start (StartDate + StartTime).");
	}
}
