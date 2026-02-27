namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

// Main content of the file
[BsonIgnoreExtraElements]
public sealed record GeoZoneInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("state_id")]
	public string StateId { get; init; } = string.Empty;

	[BsonElement("zip_code")]
	public int ZipCode { get; init; } = int.MaxValue;

	[BsonElement("state_name")]
	public string StateName { get; init; } = string.Empty;

	[BsonElement("city_name")]
	public string CityName { get; init; } = string.Empty;

	[BsonElement("county_name")]
	public string CountyName { get; init; } = string.Empty;

	[BsonElement("coordinates")]
	public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; init; } =
		GeoJson.Point(coordinates: GeoJson.Position(0.0d, 0.0d) );

	// ------------------------------------------------------------------------------------------------------<
}