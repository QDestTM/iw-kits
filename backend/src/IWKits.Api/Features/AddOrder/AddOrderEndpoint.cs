namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;

// Main content of the file
public static class AddOrderEndpoint
{
	public const string Endpoint = "/orders";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapAddOrderEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AddOrderHandlerAsync).WithName("AddOrder");
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> AddOrderHandlerAsync
	(
		[FromServices] ITaxCalculationService taxCalculator,
		[FromServices] IMongoDBContext databaseContext,
		[FromBody] AddOrderRequest request,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		// TODO: Request input validation

		// Calculate tax info to include it into the new order info
		var calculationResult = await taxCalculator.CalculateTaxAsync(
			request.Latitude, request.Longitude, request.Subtotal);

		var orderInfo = new OrderInfo()
		{
			Id = Guid.NewGuid(),

			Latitude  = request.Latitude,
			Longitude = request.Longitude,
			Subtotal  = request.Subtotal,

			CompositeTaxRate = calculationResult.CompositeTaxRate,
			TaxAmount        = calculationResult.TaxAmount,
			TotalAmount      = calculationResult.TotalAmount,
			Breakdown        = calculationResult.Breakdown,
			Jurisdictions    = calculationResult.Jurisdictions,

			Timestamp = DateTime.UtcNow
		};

		// Insert created order into the database
		await databaseContext.Orders
			.InsertOneAsync(orderInfo, null, cancellationToken);

		// Create response and send it to the client
		var respond = new AddOrderRespond(CreatedOrder: orderInfo);
		return Results.Created($"{Endpoint}/{orderInfo.Id}", respond);
	}

	// ------------------------------------------------------------------------------------------------------<
}