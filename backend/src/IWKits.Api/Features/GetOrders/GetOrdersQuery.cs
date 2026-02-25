namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.AspNetCore.Mvc;

// Main content of the file
public sealed record GetOrdersQuery
(
	[property: FromQuery(Name="cursor")]
	string? Cursor,

	[property: FromQuery(Name="limit")]
	int Limit = 20
);