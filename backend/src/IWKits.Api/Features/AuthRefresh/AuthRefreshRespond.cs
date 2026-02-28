namespace IWKits.Api.Features.AuthRefresh;

// Namespaces used by this file
using System.Text.Json.Serialization;

// Main content of the file
public sealed record AuthRefreshRespond
(
	[property: JsonPropertyName("access_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? AccessToken = null,

	[property: JsonPropertyName("refresh_token")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? RefreshToken = null,

	[property: JsonPropertyName("error_message")]
	[property: JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
	string? ErrorMessage = null
);