namespace IWKits.Api.Features.AuthRegister;

// Namespaces used by this file
using FluentValidation;

// Main content of the file
public sealed class AuthRegisterRequestValidator : AbstractValidator<AuthRegisterRequest>
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public AuthRegisterRequestValidator()
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