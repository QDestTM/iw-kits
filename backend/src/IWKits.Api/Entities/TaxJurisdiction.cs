namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Bson;

// Main content of the file
public sealed record TaxJurisdiction
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("name")]
	[JsonPropertyName("name")]
	public string Name { get; init; } = string.Empty;

	[BsonElement("type")]
	[JsonPropertyName("type")]
	public string Type { get; init; } = string.Empty;

	[BsonElement("rate")]
	[JsonPropertyName("rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal Rate { get; init; } = 0.0m;

	// ------------------------------------------------------------------------------------------------------<
}