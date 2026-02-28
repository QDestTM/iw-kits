namespace IWKits.Api.Features.AuthRefresh;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AuthRefreshRequest
(
	[property: JsonPropertyName("refresh_token")]
	string RefreshToken
);