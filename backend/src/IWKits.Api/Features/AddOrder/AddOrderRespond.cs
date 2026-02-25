namespace IWKits.Api;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AddOrderRespond
(
	[property: JsonPropertyName("created_order")]
	OrderInfo CreatedOrder
);