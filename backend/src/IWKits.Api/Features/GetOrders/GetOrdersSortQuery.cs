namespace IWKits.Api.Features.GetOrders;

// Namespaces used by this file
using Microsoft.AspNetCore.Mvc;

// Main content of the file
public sealed record GetOrdersSortQuery
(
	[property: FromQuery(Name="subtotal")]
	int Subtotal = 0,

	[property: FromQuery(Name="composite_tax_rate")]
	int CompositeTaxRate = 0,

	[property: FromQuery(Name="tax_amount")]
	int TaxAmount = 0,

	[property: FromQuery(Name="total_amount")]
	int TotalAmount = 0,

	[property: FromQuery(Name="timestamp")]
	int Timestamp = 0
);