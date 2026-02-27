namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using MongoDB.Bson;

// Main content of the file
public sealed record TaxBreakdown
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("state_rate")]
	[JsonPropertyName("state_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal StateRate { get; init; } = 0.0m;

	[BsonElement("county_rate")]
	[JsonPropertyName("county_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal CountyRate { get; init; } = 0.0m;

	[BsonElement("city_rate")]
	[JsonPropertyName("city_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal CityRate { get; init; } = 0.0m;

	[BsonElement("special_rate")]
	[JsonPropertyName("special_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal SpecialRate { get; init; } = 0.0m;

	// ------------------------------------------------------------------------------------------------------<
}