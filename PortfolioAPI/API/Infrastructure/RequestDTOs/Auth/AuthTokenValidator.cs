using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace API.Infrastructure.RequestDTOs.Auth;

public class AuthTokenValidator : AbstractValidator<AuthTokenRequest>
{
    public AuthTokenValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
