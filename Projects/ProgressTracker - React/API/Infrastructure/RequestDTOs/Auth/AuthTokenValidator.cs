using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Auth;

public class AuthTokenValidator : AbstractValidator<AuthTokenRequest>
{
    public AuthTokenValidator()
    {
        RuleFor(i => i.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long");

        RuleFor(i => i.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}