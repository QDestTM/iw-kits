namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.IO;
using CsvHelper;
using System;

// Main content of the file
public static class ImportOrdersEndpoint
{
	public const string Endpoint = "orders/import";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapImportOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, ImportOrdersHandlerAsync).WithName("ImportOrders")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<ImportOrdersRespond>(200)
			.Produces(400)
			.DisableAntiforgery();
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> ImportOrdersHandlerAsync
	(
		[FromServices] IConfiguration configuration,
		[FromServices] ITaxCalculationService taxCalculator,
		[FromServices] IMongoDBContext databaseContext,
		HttpContext httpContext,
		IFormFile file,
		CancellationToken cancellationToken)
	{
		if ( file is null || file.Length == 0 )
		{
			return Results.BadRequest("File is empty or missing.");
		}

		// Get size of batch for importing
		int importBatchSize = Math.Max
		(
			val1: configuration.GetValue<int>("Constraints:ImportBatchSize"),
			val2: 1000
		);

		// Variable to count imported records in total
		int importedTotal = 0;

		// Lists to store imported orders and errors
		var imported = new List<OrderInfo>(capacity: importBatchSize);
		var errors   = new List<string>   (capacity: 4);

		// Create file reader and use it to create csv reader instance
		using var reader = new StreamReader( file.OpenReadStream() );
		using var csv    = new CsvReader(reader, CultureInfo.InvariantCulture);

		// Read record from csv file line by line using asyncronouns reader
		var asyncOrdersReader = csv.GetRecordsAsync<OrderInfoCsv>(cancellationToken);

		await foreach ( var csvOrder in asyncOrdersReader )
		{
			try
			{
				var calculationResult = await taxCalculator.CalculateTaxAsync
				(
					latitude : csvOrder.Latitude,
					longitude: csvOrder.Longitude,
					subtotal : csvOrder.Subtotal
				);

				// Create new record of order info
				var order = new OrderInfo
				{
					Id = Utils.GuidFrom(csvOrder.Id),

					Latitude  = csvOrder.Latitude,
					Longitude = csvOrder.Longitude,
					Subtotal  = csvOrder.Subtotal,

					CompositeTaxRate = calculationResult.CompositeTaxRate,
					TaxAmount        = calculationResult.TaxAmount,
					TotalAmount      = calculationResult.TotalAmount,
					Breakdown        = calculationResult.Breakdown,
					Jurisdictions    = calculationResult.Jurisdictions,

					Timestamp = csvOrder.Timestamp
				};

				// Store order to the list and increase counter
				imported.Add(order); importedTotal++;
			}
			catch ( Exception exception )
			{
				errors.Add($"Error processing row: {exception.Message}");
			}

			// Store orders when batch if filled
			if ( imported.Count >= importBatchSize )
			{
				await databaseContext.Orders.InsertManyAsync(imported, null, cancellationToken);
				imported.Clear();
			}
		}

		// Store orders that can remain in imported
		if ( imported.Count != 0 )
		{
			await databaseContext.Orders.InsertManyAsync(imported, null, cancellationToken);
		}

		// Send respond with results to the client
		var respond = new ImportOrdersRespond(importedTotal, errors);
		return Results.Ok(respond);
	}

	// ------------------------------------------------------------------------------------------------------<
}