namespace IWKits.Api.Features.CreateOrder;

// Namespaces used by this file
using FluentValidation;

// Main content of the file
public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public CreateOrderRequestValidator()
	{
		RuleFor(x => x.Longitude)
			.InclusiveBetween(-180, 180)
			.WithMessage("Invalid longitude.");

		RuleFor(x => x.Latitude)
			.InclusiveBetween(-90, 90)
			.WithMessage("Invalid latitude.");

		RuleFor(x => x.Subtotal)
			.GreaterThan(0)
			.WithMessage("Subtotal must be greater than zero.");
	}

	// ------------------------------------------------------------------------------------------------------<
}