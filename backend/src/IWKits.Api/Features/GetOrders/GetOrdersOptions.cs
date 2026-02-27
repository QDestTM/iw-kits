namespace IWKits.Api.Features.GetOrders;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System;

// Main content of the file
public sealed record GetOrdersOptions
(
	[property: JsonPropertyName("min_total_amount")]
	decimal? MinTotalAmount,
	[property: JsonPropertyName("max_total_amount")]
	decimal? MaxTotalAmount,

	[property: JsonPropertyName("from_date")]
	DateTime? FromDate,
	[property: JsonPropertyName("to_date")]
	DateTime? ToDate,

	[property: JsonPropertyName("sort_by")]
	string SortBy,
	[property: JsonPropertyName("descending")]
	bool Descending,

	[property: JsonPropertyName("page_size")]
	int PageSize,
	[property: JsonPropertyName("page")]
	int Page
);