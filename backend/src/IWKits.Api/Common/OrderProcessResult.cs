namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Diagnostics.CodeAnalysis;
using IWKits.Api.Entities;

// Main content of the file
public sealed record OrderProcessResult
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance predicates
	[MemberNotNullWhen(false, nameof(OrderInfo))]
	[MemberNotNullWhen(true, nameof(ErrorMessage))]
	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

	// Public instance properties
	public string? ErrorMessage { get; init; }
	public OrderInfo? OrderInfo { get; init; }

	// # ----------------------------------------------------------------------------------------------------<

	public static OrderProcessResult Success(OrderInfo order) => new() { OrderInfo = order };

	public static OrderProcessResult Failure(string errorMsg) => new() { ErrorMessage = errorMsg };

	// ------------------------------------------------------------------------------------------------------<
}