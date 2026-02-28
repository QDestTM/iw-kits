namespace IWKits.Api.Features.GetOrders;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System.Collections.Generic;
using IWKits.Api.Entities;

// Main content of the file
public sealed record GetOrdersRespond
(
	[property: JsonPropertyName("items")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	List<OrderInfo>? Items = null,

	[property: JsonPropertyName("total_count")]
	long TotalCount = 0,

	[property: JsonPropertyName("total_pages")]
	long TotalPages = 0
);