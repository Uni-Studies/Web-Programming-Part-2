using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Users;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(i => i.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long");

        RuleFor(i => i.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

        RuleFor(i => i.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(i => i.LastName)
            .NotEmpty().WithMessage("Last name is required");
    }
}