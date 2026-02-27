namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

// Main content of the file
[BsonIgnoreExtraElements]
public sealed record TaxRateInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("zip_code")]
	public int ZipCode { get; init; } = int.MaxValue;

	[BsonElement("state_id")]
	public string StateId { get; init; } = string.Empty;

	[BsonElement("state_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal StateRate { get; init; } = 0.0m;

	[BsonElement("estimated_combined_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal EstimatedCombinedRate { get; init; } = 0.0m;

	[BsonElement("estimated_county_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal EstimatedCountyRate { get; init; } = 0.0m;

	[BsonElement("estimated_city_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal EstimatedCityRate { get; init; } = 0.0m;

	[BsonElement("estimated_special_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal EstimatedSpecialRate { get; init; } = 0.0m;

	// ------------------------------------------------------------------------------------------------------<
}