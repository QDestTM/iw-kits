namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

// Main content of the file
[BsonIgnoreExtraElements]
public sealed record GeoZoneInfoFull : GeoZoneInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("coordinates")]
	public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; init; } =
		GeoJson.Point(coordinates: GeoJson.Position(0.0d, 0.0d) );

	// ------------------------------------------------------------------------------------------------------<
}