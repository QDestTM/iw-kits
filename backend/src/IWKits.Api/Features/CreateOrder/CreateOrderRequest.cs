namespace IWKits.Api.Features.CreateOrder;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record CreateOrderRequest
(
	[property: JsonPropertyName("longitude")]
	double Longitude,

	[property: JsonPropertyName("latitude")]
	double Latitude,

	[property: JsonPropertyName("subtotal")]
	decimal Subtotal
);