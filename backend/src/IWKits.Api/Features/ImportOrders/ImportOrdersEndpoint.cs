namespace IWKits.Api.Features.ImportOrders;

// Namespaces used by this file
using Microsoft.AspNetCore.Authorization;
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
using MongoDB.Driver;
using System.Linq;
using System.IO;
using CsvHelper;

// Main content of the file
public static class ImportOrdersEndpoint
{
	public const string Endpoint = "orders/import";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapImportOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, ImportOrdersHandlerAsync).WithName("ImportOrders")
			.RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
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

		// Get chunk size from the options constrains section
		int importChunkSize = constraints.Value.ImportChunkSize;

		// Allow unordered insertion to speed up insertion process
		var insertOptions = new InsertManyOptions() { IsOrdered = false };

		// List for errors and total imports counter
		var errors = new List<string>();
		int importedTotal = 0;

		// Create file reader and use it to create csv reader instance
		using var reader = new StreamReader( file.OpenReadStream() );
		using var csv    = new CsvReader(reader, CultureInfo.InvariantCulture);

		// Read record from csv file line by line using asyncronouns reader
		var chunks = csv.GetRecordsAsync<RawOrderInfo>(ct).Chunk(importChunkSize);
		var options = new ParallelOptions() { CancellationToken = ct, MaxDegreeOfParallelism = 2 };

		await Parallel.ForEachAsync(chunks, options, async (chunk, token) =>
		{
			var tasks = chunk.Select(orderProcess.ProcessAsync);
			var toInsert = new List<OrderInfo>(importChunkSize);

			// Filter results for error message and order infos
			foreach (var result in await Task.WhenAll(tasks))
			{
				if ( result.HasError )
				{
					errors.Add(result.ErrorMessage);
				}
				else
				{
					toInsert.Add(result.OrderInfo);
				}
			}

			// Insert orders and atomicaly increase total counter
			if ( toInsert.Count > 0 )
			{
				await dataDatabase.Orders.InsertManyAsync(toInsert, insertOptions, token);
				Interlocked.Add(ref importedTotal, toInsert.Count);
			}
		});

		// Send respond with results to the client
		var respond = new ImportOrdersRespond(importedTotal, errors);
		return Results.Ok(respond);
	}

	// ------------------------------------------------------------------------------------------------------<
}