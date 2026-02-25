namespace IWKits.Api;

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
	public decimal StateRate { get; init; } = 0.0m;

	[BsonElement("country_rate")]
	[JsonPropertyName("country_rate")]
	public decimal CountryRate { get; init; } = 0.0m;

	[BsonElement("city_rate")]
	[JsonPropertyName("city_rate")]
	public decimal CityRate { get; init; } = 0.0m;

	[BsonElement("special_rate")]
	[JsonPropertyName("special_rate")]
	public decimal SpecialRate { get; init; } = 0.0m;

	// ------------------------------------------------------------------------------------------------------<
}