using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.User;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        // AuthUserId validation - must reference valid AuthUser
        RuleFor(x => x.AuthUserId)
            .GreaterThan(0).WithMessage("AuthUserId must be a positive integer.");

        // FirstName validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(100).WithMessage("FirstName must be at most 100 characters.")
            .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("FirstName can only contain letters, spaces, hyphens, and apostrophes.");

        // LastName validation
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100).WithMessage("LastName must be at most 100 characters.")
            .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("LastName can only contain letters, spaces, hyphens, and apostrophes.");

        // Sex validation
        RuleFor(x => x.Sex)
            .MaximumLength(50).WithMessage("Sex must be at most 50 characters.");

        // BirthDate validation
        RuleFor(x => x.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("BirthDate cannot be in the future.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-120))).WithMessage("BirthDate cannot be more than 120 years ago.");

        // BirthCity validation
        RuleFor(x => x.BirthCity)
            .MaximumLength(100).WithMessage("BirthCity must be at most 100 characters.");

        // Address validation
        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address must be at most 200 characters.");

        // Country validation
        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country must be at most 100 characters.");

        // Nationality validation
        RuleFor(x => x.Nationality)
            .MaximumLength(100).WithMessage("Nationality must be at most 100 characters.");

        // Details validation
        RuleFor(x => x.Details)
            .MaximumLength(5000).WithMessage("Details must be at most 5000 characters.");

        // ProfilePicture validation (URL or path)
        RuleFor(x => x.ProfilePicture)
            .MaximumLength(500).WithMessage("ProfilePicture path must be at most 500 characters.");
    }
}
