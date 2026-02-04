using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Interest;

public class InterestValidator : AbstractValidator<InterestRequest>
{
    public InterestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must be at most 200 characters.");
    }
}
