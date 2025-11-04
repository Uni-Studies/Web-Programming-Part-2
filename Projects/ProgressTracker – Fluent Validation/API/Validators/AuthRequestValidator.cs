using System;
using System.Security.Cryptography.X509Certificates;
using API.Infrastructure.RequestDTOs.Auth;
using API.Infrastructure.RequestDTOs.Users;
using Common.Entities;
using FluentValidation;

namespace API.Validators;

public class AuthRequestValidator : AbstractValidator<AuthTokenRequest>
{
     public AuthRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
