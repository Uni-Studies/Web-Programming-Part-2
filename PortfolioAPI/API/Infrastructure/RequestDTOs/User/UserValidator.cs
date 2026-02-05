using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.User;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(100).WithMessage("FirstName must be at most 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100).WithMessage("LastName must be at most 100 characters.");
        RuleFor(x => x.Sex)
            .MaximumLength(50).WithMessage("Sex must be at most 50 characters.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("BirthDate cannot be in the future.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-120))).WithMessage("BirthDate cannot be more than 120 years ago.");

        RuleFor(x => x.BirthCity)
            .MaximumLength(100).WithMessage("BirthCity must be at most 100 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address must be at most 200 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country must be at most 100 characters.");

        RuleFor(x => x.Nationality)
            .MaximumLength(100).WithMessage("Nationality must be at most 100 characters.");

        RuleFor(x => x.Details)
            .MaximumLength(5000).WithMessage("Details must be at most 5000 characters.");
    }
}
