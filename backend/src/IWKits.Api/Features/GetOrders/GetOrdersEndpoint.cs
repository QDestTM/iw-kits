namespace IWKits.Api.Features.GetOrders;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using System.Threading;
using MongoDB.Driver;
using System;

// Main content of the file
public static class GetOrdersEndpoint
{
	public const string Endpoint = "/orders";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapGetOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapGet(Endpoint, GetOrdersHandlerAsync).WithName("GetOrders")
			.Produces<GetOrdersRespond>(200);
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> GetOrdersHandlerAsync
	(
		[FromServices] DataDatabaseContext dataDatabase,
		[AsParameters] GetOrdersFindQuery filterOptions,
		[AsParameters] GetOrdersSortQuery sortOptions,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var sortDefinition = CreateSortDefinition(sortOptions);
		return Results.Ok();
	}

	// ------------------------------------------------------------------------------------------------------<

	private static SortDefinition<OrderInfo> CreateSortDefinition(GetOrdersSortQuery options)
	{
		var definitions = new List<SortDefinition<OrderInfo>>();
		var builder = Builders<OrderInfo>.Sort;

		// Create array of expression and sort direction values to process
		(int, Expression<Func<OrderInfo, object>>)[] definitionsSettings =
		[
			(options.Subtotal,         x => x.Timestamp),
			(options.CompositeTaxRate, x => x.CompositeTaxRate),
			(options.TaxAmount,        x => x.TaxAmount),
			(options.TotalAmount,      x => x.TotalAmount),
			(options.Timestamp,        x => x.Timestamp)
		];

		foreach ( var (direction, field) in definitionsSettings )
		{
			if ( direction == 0 ) continue;

			// Add sort based on provided direction
			definitions.Add
			(
				(direction == 1)
					? builder. Ascending(field)
					: builder.Descending(field)
			);
		}

		// Sort by date DESCENDING if no sorting options provided
		if ( definitions.Count == 0 )
		{
			return builder.Descending(x => x.Timestamp);
		}

		// FOR PAGINATION: to prevent inconsistent sorting result
		definitions.Add( builder.Ascending(x => x.Id) );

		// Combine created sort definitions
		return builder.Combine(definitions);
	}


	private static FilterDefinition<OrderInfo> CreateFilterDefinition(GetOrdersFindQuery options)
	{
		return Builders<OrderInfo>.Filter.Gt(x => x.Timestamp, DateTime.UtcNow);
	}

	// ------------------------------------------------------------------------------------------------------<
}