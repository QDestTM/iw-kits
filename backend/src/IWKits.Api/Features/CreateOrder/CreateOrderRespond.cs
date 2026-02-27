namespace IWKits.Api.Features.CreateOrder;

// Namespaces used by this file
using System.Text.Json.Serialization;
using IWKits.Api.Entities;

// Main content of the file
public sealed record CreateOrderRespond
(
	[property: JsonPropertyName("created_order")]
	OrderInfo? CreatedOrder = null,

	[property: JsonPropertyName("error_message")]
	string? ErrorMessage = null
);