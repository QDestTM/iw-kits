namespace IWKits.Api.Features.AuthLogin;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AuthLoginRequest
(
	[property: JsonPropertyName("username")]
	string Username,

	[property: JsonPropertyName("password")]
	string Password
);