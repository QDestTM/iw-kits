namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

// Main content of the file
public static class GetOrdersEndpoint
{
	public const string Endpoint = "/orders";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapGetOrdersEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapGet(Endpoint, GetOrdersHandlerAsync).WithName("GetOrders");
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> GetOrdersHandlerAsync
	(
		[FromServices] IMongoDBContext databaseContext,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		return Results.Ok();
	}

	// ------------------------------------------------------------------------------------------------------<
}