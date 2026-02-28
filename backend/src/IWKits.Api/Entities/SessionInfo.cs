namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using System;

// Main content of the file
public sealed record SessionInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("_id")]
	[JsonPropertyName("id")]
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; init; } = Guid.Empty;

	[BsonElement("user_id")]
	[JsonPropertyName("user_id")]
	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; init; } = Guid.Empty;

	[BsonElement("refresh_token")]
	[JsonPropertyName("refresh_token")]
	public string RefreshToken { get; init; } = string.Empty;

	[BsonElement("expires_at")]
	[JsonPropertyName("expires_at")]
	[BsonDateTimeOptions(Kind=DateTimeKind.Utc)]
	public DateTime ExpiresAt { get; init; } = DateTime.MinValue;

	// ------------------------------------------------------------------------------------------------------<
}