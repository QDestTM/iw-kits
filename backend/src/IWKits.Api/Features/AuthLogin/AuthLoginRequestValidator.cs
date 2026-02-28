namespace IWKits.Api.Features.AuthLogin;

// Namespaces used by this file
using FluentValidation;

// Main content of the file
public sealed class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public AuthLoginRequestValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty().NotNull()
			.WithMessage("Username is required.");

		RuleFor(x => x.Password)
			.NotEmpty().NotNull()
			.WithMessage("Password is required.");
	}

	// ------------------------------------------------------------------------------------------------------<
}