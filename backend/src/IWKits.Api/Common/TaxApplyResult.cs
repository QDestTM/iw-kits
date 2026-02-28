namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using IWKits.Api.Entities;

// Main content of the file
public sealed record TaxApplyResult
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance predicates
	[MemberNotNullWhen(true, nameof(ErrorMessage))]
	public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

	// Public instance properties
	public decimal CompositeTaxRate { get; init; } = 0.0m;

	public decimal TotalAmount { get; init; } = 0.0m;
	public decimal TaxAmount   { get; init; } = 0.0m;

	public TaxBreakdown Breakdown              { get; init; } = new();
	public List<TaxJurisdiction> Jurisdictions { get; init; } = [];

	public string ErrorMessage { get; init; } = string.Empty;

	// # ----------------------------------------------------------------------------------------------------<

	public static TaxApplyResult Failure(string errorMsg) => new() { ErrorMessage = errorMsg };

	// ------------------------------------------------------------------------------------------------------<
}