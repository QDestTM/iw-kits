namespace IWKits.Api;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AddOrderRequest
(
	[property: JsonPropertyName("latitude")]
	double Latitude,

	[property: JsonPropertyName("longitude")]
	double Longitude,

	[property: JsonPropertyName("subtotal")]
	decimal Subtotal
);