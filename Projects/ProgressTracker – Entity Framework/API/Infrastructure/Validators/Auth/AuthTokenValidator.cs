using FluentValidation;
using API.Infrastructure.RequestDTOs.Auth;

namespace API.Infrastructure.Validators.Auth;

public class AuthTokenValidator : AbstractValidator<AuthTokenRequest>
{
    public AuthTokenValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
    }
}