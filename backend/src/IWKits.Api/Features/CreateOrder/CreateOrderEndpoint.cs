namespace IWKits.Api.Features.CreateOrder;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Services;
using IWKits.Api.Database;
using System.Threading;
using System;

// Main content of the file
public static class CreateOrderEndpoint
{
	public const string Endpoint = "/orders";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapCreateOrderEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, CreateOrderHandlerAsync)
			.Produces<CreateOrderRespond>(200)
			.WithName("CreateOrder");
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> CreateOrderHandlerAsync
	(
		[FromServices] IOrderProcessService orderProcess,
		[FromServices] DataDatabaseContext dataDatabase,
		[FromBody] CreateOrderRequest request,
		HttpContext httpContext, CancellationToken ct)
	{
		// TODO: Request input validation

		// Calculate tax info to include it into the new order info
		var rawOrderInfo = new RawOrderInfo()
		{
			Id = 0,

			Longitude = request.Longitude,
			Latitude  = request.Latitude,
			Subtotal  = request.Subtotal,
			Timestamp = DateTime.UtcNow
		};

		// Use order process service to apply taxes to the creater raw order
		var processResult = await orderProcess.ProcessAsync(rawOrderInfo);

		// Return response with error message if one is defined
		if ( processResult.HasError )
		{
			var respond = new CreateOrderRespond(null, processResult.ErrorMessage);
			return Results.Ok(respond);
		}
		else
		{
			OrderInfo orderInfo = processResult.OrderInfo;

			// Insert created order into the database
			await dataDatabase.Orders.InsertOneAsync(orderInfo, null, ct);

			// Create response and send it to the client
			var respond = new CreateOrderRespond(orderInfo, null);
			return Results.Created($"{Endpoint}/{orderInfo.Id}", respond);
		}
	}

	// ------------------------------------------------------------------------------------------------------<
}