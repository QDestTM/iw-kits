namespace IWKits.Api.Features.CreateOrder;

// Namespaces used by this file
using System.Text.Json.Serialization;
using IWKits.Api.Entities;

// Main content of the file
public sealed record CreateOrderRespond
(
	[property: JsonPropertyName("created_order")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	OrderInfo? CreatedOrder = null,

	[property: JsonPropertyName("error_message")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? ErrorMessage = null
);