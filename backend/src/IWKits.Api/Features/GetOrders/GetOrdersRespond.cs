namespace IWKits.Api;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System.Collections.Generic;

// Main content of the file
public sealed record GetOrdersRespond
(
	[property: JsonPropertyName("items")]
	List<OrderInfo> Items
);