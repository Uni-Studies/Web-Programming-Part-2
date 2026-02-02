using System;
using FluentValidation;

namespace API.Infrastructure.RequestDTOs.SocialNetwork;

public class SocialNetworkValidator : AbstractValidator<SocialNetworkRequest>
{
    public SocialNetworkValidator()
    {
        // Type validation
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(100).WithMessage("Type must be at most 100 characters.")
            .Matches(@"^[a-zA-Z0-9\s-]+$").WithMessage("Type can only contain letters, numbers, spaces, and hyphens.");

        // Account validation
        RuleFor(x => x.Account)
            .NotEmpty().WithMessage("Account is required.")
            .MaximumLength(200).WithMessage("Account must be at most 200 characters.");

        // Link validation
        RuleFor(x => x.Link)
            .NotEmpty().WithMessage("Link is required.")
            .Must(IsValidUrl).WithMessage("Link must be a valid URL.");
    }

    private bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
