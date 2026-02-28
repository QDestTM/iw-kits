namespace IWKits.Api.Features.AuthRefresh;

// Namespaces used by this file
using FluentValidation;

// Main content of the file
public sealed class AuthRefreshRequestValidator : AbstractValidator<AuthRefreshRequest>
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public AuthRefreshRequestValidator()
	{
		RuleFor(x => x.RefreshToken)
			.NotEmpty().NotNull()
			.WithMessage("Refresh token is required.");
	}

	// ------------------------------------------------------------------------------------------------------<
}