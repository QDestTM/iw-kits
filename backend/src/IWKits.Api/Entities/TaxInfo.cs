namespace IWKits.Api;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using MongoDB.Bson;

// Main content of the file
public sealed record TaxInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("composite_tax_rate")]
	[JsonPropertyName("composite_tax_rate")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal CompositeTaxRate { get; init; } = 0.0m;

	[BsonElement("tax_amount")]
	[JsonPropertyName("tax_amount")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal TaxAmount { get; init; } = 0.0m;

	[BsonElement("total_amount")]
	[JsonPropertyName("total_amount")]
	[BsonRepresentation(BsonType.Decimal128)]
	public decimal TotalAmount { get; init; } = 0.0m;

	[BsonElement("breakdown")]
	[JsonPropertyName("breakdown")]
	public TaxBreakdown Breakdown { get; init; } = new();

	[BsonElement("jurisdictions")]
	[JsonPropertyName("jurisdictions")]
	public List<TaxJurisdiction> Jurisdictions { get; init; } = [];

	// ------------------------------------------------------------------------------------------------------<
}