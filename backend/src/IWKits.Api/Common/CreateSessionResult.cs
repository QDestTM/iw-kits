namespace IWKits.Api.Services;

// Namespaces used by this file
using IWKits.Api.Entities;

// Main content of the file
public sealed record CreateSessionResult
(
	SessionInfo Session,
	string AccessToken
);