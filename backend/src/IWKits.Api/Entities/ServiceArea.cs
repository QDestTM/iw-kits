namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

// Main content of the file
[BsonIgnoreExtraElements]
public record ServiceArea
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("state_id")]
	public string StateId { get; init; } = string.Empty;

	// ------------------------------------------------------------------------------------------------------<
}