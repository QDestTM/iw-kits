namespace IWKits.Api.Features.AuthRegister;

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
public static class AuthRegisterEndpoint
{
	public const string Endpoint = "/auth/register";

	// ^ ----------------------------------------------------------------------------------------------------<

	public static void MapAuthRegisterEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapPost(Endpoint, AuthRegisterHandlerAsync)
			.Produces<AuthRegisterRespond>(200)
			.Produces(400)
			.WithName("AuthRegister");
	}

	// @ ----------------------------------------------------------------------------------------------------<

	private static async Task<IResult> AuthRegisterHandlerAsync
	(
		[FromServices] IOptions<SecuritySettings> securitySettings,
		[FromServices] AuthDatabaseContext authDatabase,
		[FromServices] ISecurityService securityService,
		[FromServices] ISessionService sessionService,
		[FromBody] AuthRegisterRequest request,
		CancellationToken ct)
	{
		var filter = Builders<UserInfo>.Filter.Eq(x => x.Username, request.Username);
		var userExists = await authDatabase.Users.Find(filter).AnyAsync(ct);

		if ( userExists )
		{
			var respond = new AuthRegisterRespond(null, null, null, "Username already taken");
			return Results.Json(respond, statusCode: 400);
		}

		// Create info for new user and cache it's password
		var hashpass = securityService.HashPassword(request.Password);

		var userInfo = new UserInfo()
		{
			Id       = Guid.NewGuid(),
			Username = request.Username,
			Password = hashpass,
			Role     = "user"
		};

		// Store created user info to the database
		await authDatabase.Users.InsertOneAsync(userInfo, null, ct);

		// Create session info object using session service and received user info
		var createResult = await sessionService.CreateSessionAsync(userInfo, ct);

		return Results.Ok<AuthRegisterRespond>(new
		(
			RefreshToken: createResult.Session.RefreshToken,
			AccessToken : createResult.AccessToken,
			User        : userInfo
		));
	}

	// ------------------------------------------------------------------------------------------------------<
}