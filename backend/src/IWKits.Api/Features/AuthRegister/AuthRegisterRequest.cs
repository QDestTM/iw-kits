namespace IWKits.Api.Features.AuthRegister;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AuthRegisterRequest
(
	[property: JsonPropertyName("username")]
	string Username,

	[property: JsonPropertyName("password")]
	string Password
);