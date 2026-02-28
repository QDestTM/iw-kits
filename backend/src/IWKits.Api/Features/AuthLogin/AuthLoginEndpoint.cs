namespace IWKits.Api.Features.AuthLogin;

// Namespaces used by this file
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IWKits.Api.Database;
using IWKits.Api.Services;
using IWKits.Api.Entities;
using IWKits.Api.Settings;
using System.Threading;
using MongoDB.Driver;
using System;

// Main content of the file
public static class AuthLoginEndpoint
{
	public const string Endpoint = "/auth/login";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapAuthLoginEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AuthLoginHandlerAsync)
			.Produces<AuthLoginRespond>(200)
			.Produces(401)
			.WithName("AuthLogin");
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> AuthLoginHandlerAsync
	(
		[FromServices] IOptions<SecuritySettings> securitySettings,
		[FromServices] AuthDatabaseContext authDatabase,
		[FromServices] ISecurityService securityService,
		[FromServices] ISessionService sessionService,
		[FromBody] AuthLoginRequest request,
		HttpContext httpContext, CancellationToken ct)
	{
		var filter = Builders<UserInfo>.Filter.Eq(x => x.Username, request.Username);
		var userInfo = await authDatabase.Users.Find(filter).FirstOrDefaultAsync(ct);

		// Return 401 Unauthorized if user not found or password verification is failed
		if ( userInfo is null || !securityService.VerifyPassword(userInfo.Password, request.Password) )
		{
			var respond = new AuthLoginRespond(null, null, null, "Invalid credentials");
			return Results.Json(respond, statusCode: 401);
		}

		// Create session info object using session service and received user info
		var createResult = await sessionService.CreateSessionAsync(userInfo, ct);

		return Results.Ok<AuthLoginRespond>(new
		(
			RefreshToken: createResult.Session.RefreshToken,
			AccessToken : createResult.AccessToken,
			User        : userInfo
		));
	}

	// ------------------------------------------------------------------------------------------------------<
}