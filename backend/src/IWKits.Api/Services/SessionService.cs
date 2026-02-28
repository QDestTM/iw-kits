namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Threading.Tasks;
using IWKits.Api.Settings;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using System.Threading;
using MongoDB.Driver;
using System;

// Main content of the file
public sealed class SessionService : ISessionService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	private readonly AuthDatabaseContext authDatabase;
	private readonly SessionSettings sessionSettings;
	private readonly ISecurityService securityService;

	// Public instance constructors
	public SessionService(ISecurityService securityService,
		SessionSettings sessionSettings, AuthDatabaseContext authDatabase)
	{
		this.sessionSettings = sessionSettings;
		this.securityService = securityService;
		this.authDatabase = authDatabase;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<CreateSessionResult> CreateSessionAsync(UserInfo userInfo, CancellationToken ct)
	{
		var accessToken = securityService.GenerateAccessToken(userInfo);
		var refreshToken = securityService.GenerateRefreshToken();

		// Find refresh token expiration datetime
		var expiresAt = DateTime.UtcNow.AddMinutes
			(sessionSettings.RefreshPeriod);

		// Create new session info using tokens and date
		var session = new SessionInfo()
		{
			UserId = userInfo.Id,
			RefreshToken = refreshToken,
			ExpiresAt = expiresAt
		};

		// Store created session and return successfull result
		await authDatabase.Sessions.InsertOneAsync(session, null, ct);
		return new CreateSessionResult(session, accessToken);
	}


	public async Task<RefreshSessionResult> RefreshSessionAsync(string refreshToken, CancellationToken ct)
	{
		var sessionFilter = Builders<SessionInfo>.Filter.Eq(s => s.RefreshToken, refreshToken);
		var oldSession = await authDatabase.Sessions.Find(sessionFilter).FirstOrDefaultAsync(ct);

		// Fail refreshing if session is not found
		if ( oldSession is null )
		{
			return RefreshSessionResult.Failure("Session not found");
		}

		// Fail refreshing if used token is expired
		if ( oldSession.ExpiresAt < DateTime.UtcNow )
		{
			await authDatabase.Sessions.DeleteOneAsync(sessionFilter, ct);
			return RefreshSessionResult.Failure("Session expired");
		}

		// Find user from old session to generate a new access token
		var userFilter = Builders<UserInfo>.Filter.Eq(x => x.Id, oldSession.UserId);
		var user = await authDatabase.Users.Find(userFilter).FirstOrDefaultAsync(ct);

		if ( user is null )
		{
			return RefreshSessionResult.Failure("User associated with session not found");
		}

		// Delete old session and create new using service tool
		await authDatabase.Sessions.DeleteOneAsync(sessionFilter, ct);
		var createResult = await CreateSessionAsync(user, ct);

		return RefreshSessionResult.Success(
			createResult.Session, createResult.AccessToken);
	}

	// ------------------------------------------------------------------------------------------------------<
}