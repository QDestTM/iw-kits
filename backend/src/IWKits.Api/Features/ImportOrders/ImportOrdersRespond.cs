namespace IWKits.Api.Features.ImportOrders;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System.Collections.Generic;

// Main content of the file
public sealed record ImportOrdersRespond
(
	[property: JsonPropertyName("imported_total")]
	int ImportedTotal,

	[property: JsonPropertyName("errors")]
	List<string> Errors
);