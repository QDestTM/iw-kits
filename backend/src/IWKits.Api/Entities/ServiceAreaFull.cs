namespace IWKits.Api.Entities;

// Namespaces used by this file
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

// Main content of the file
[BsonIgnoreExtraElements]
public sealed record ServiceAreaFull : ServiceArea
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[BsonElement("boundary")]
	public GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> Boundary { get; init; }
		= GeoJson.MultiPolygon(
			GeoJson.PolygonCoordinates(
				GeoJson.Geographic(0.0d, 0.0d),
				GeoJson.Geographic(1.0d, 0.0d),
				GeoJson.Geographic(0.0d, 1.0d),
				GeoJson.Geographic(0.0d, 0.0d)
			)
		);

	// ------------------------------------------------------------------------------------------------------<
}