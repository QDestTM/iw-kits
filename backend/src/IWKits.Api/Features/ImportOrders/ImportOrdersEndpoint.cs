namespace IWKits.Api.Features.ImportOrders;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using IWKits.Api.Entities;
using IWKits.Api.Services;
using IWKits.Api.Database;
using IWKits.Api.Settings;
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
		[FromServices] IOptions<ConstraintSettings> constraints,
		[FromServices] IOrderProcessService orderProcess,
		[FromServices] DataDatabaseContext dataDatabase,
		HttpContext httpContext, IFormFile file, CancellationToken ct)
	{
		if ( file is null || file.Length == 0 )
		{
			return Results.BadRequest("File is empty or missing.");
		}

		// Get size of batch for importing
		int importBatchSize = constraints.Value.ImportBatchSize;

		// Variable to count imported records in total
		int importedTotal = 0;

		// Lists to store imported orders and errors
		var imported = new List<OrderInfo>();
		var errors   = new List<string>   ();

		// Create file reader and use it to create csv reader instance
		using var reader = new StreamReader( file.OpenReadStream() );
		using var csv    = new CsvReader(reader, CultureInfo.InvariantCulture);

		// Read record from csv file line by line using asyncronouns reader
		var asyncOrdersReader = csv.GetRecordsAsync<RawOrderInfo>(ct);

		await foreach ( var rawOrder in asyncOrdersReader )
		{
			var processResult = await orderProcess.ProcessAsync(rawOrder);

			// Store message for error that may occur at processing
			if ( processResult.HasError )
			{
				errors.Add(processResult.ErrorMessage); continue;
			}

			// Store order to the list and increase counter
			imported.Add(processResult.OrderInfo); importedTotal++;

			// Store orders when batch if filled
			if ( imported.Count >= importBatchSize )
			{
				await dataDatabase.Orders.InsertManyAsync(imported, null, ct);
				imported.Clear();
			}
		}

		// Store orders that can remain in imported
		if ( imported.Count != 0 )
		{
			await dataDatabase.Orders.InsertManyAsync(imported, null, ct);
		}

		// Send respond with results to the client
		var respond = new ImportOrdersRespond(importedTotal, errors);
		return Results.Ok(respond);
	}

	// ------------------------------------------------------------------------------------------------------<
}