namespace IWKits.Api.Services;

// Namespaces used by this file
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.GeoJsonObjectModel;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using IWKits.Api.Common;
using MongoDB.Driver;
using System.Linq;
using System;

// Main content of the file
public sealed class GeoLocationService : IGeoLocationService
{
	const string ServiceAreasCacheKey = "all_service_areas";

	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	private readonly CoreDatabaseContext coreDatabase;
	private readonly IMemoryCache cache;

	// Public instance constructors
	public GeoLocationService(CoreDatabaseContext coreDatabase, IMemoryCache cache)
	{
		this.coreDatabase = coreDatabase;
		this.cache = cache;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<ServiceArea?> FindServiceAreaAsync(Point coordinates)
	{
		// Receive wrapped service areas from the cache
		var wrappedAreas = cache.Get<List<ServiceAreaXNTS>>
			(ServiceAreasCacheKey) ?? [];

		// Find area which contains provided coordinate
		foreach ( var areaXNTS in wrappedAreas )
		{
			if ( areaXNTS.NtsBoundary.Contains(coordinates) )
			{
				return areaXNTS.ServiceArea;
			}
		}

		return null; // No intersecions with point
	}


	public async Task<GeoZoneInfo?> FindGeoZoneInfoAsync(Point coordinates, string state)
	{
		var gcoordinates = GeoJson.Geographic(coordinates.X, coordinates.Y);

		// Create geo zone info filter builder
		var filterBuilder = Builders<GeoZoneInfoFull>.Filter;

		// Create filter based on zip code value
		var geoZoneFilter = filterBuilder.And
		(
			filterBuilder.NearSphere(x => x.Coordinates, GeoJson.Point(gcoordinates)),
			filterBuilder.        Eq(x => x.StateId, state)
		);

		// Use created filter to find first suitable element
		return await coreDatabase.GeoZones
			.Find(geoZoneFilter)
			.Limit(limit: 1)
			.As<GeoZoneInfo>()
			.FirstOrDefaultAsync();
	}


	public async Task<TaxRateInfo?> FindTaxRateInfoAsync(int zipCode, string state)
	{
		string key = $"{nameof(TaxRateInfo)}_{state}_{zipCode}";

		// Use cache to get or fetch tax rate info from zipCode and state
		return await cache.GetOrCreateAsync(key, async (entry) =>
		{
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
			entry.Priority = CacheItemPriority.High;

			// Create tax rate info filter builder
			var filterBuilder = Builders<TaxRateInfoFull>.Filter;

			// Create filter based on zip code value
			var taxRateFilter = filterBuilder.And
			(
				filterBuilder.Eq(x => x.ZipCode, zipCode),
				filterBuilder.Eq(x => x.StateId, state)
			);

			// Use created filter to find first suitable element
			return await coreDatabase.TaxRates
				.Find(taxRateFilter)
				.Limit(limit: 1)
				.As<TaxRateInfo>()
				.FirstOrDefaultAsync();
		});
	}

	// ------------------------------------------------------------------------------------------------------<

	public async Task RefreshGeoLocationCache()
	{
		var options = new MemoryCacheEntryOptions()
		{
			Priority = CacheItemPriority.High
		};

		// Find all areas and create list for area+nts poly pairs
		var areas = coreDatabase.SerAreas.Find(_ => true);
		var wrapperAreas = new List<ServiceAreaXNTS>();

		// Convert geo-json multi polygons into NTS MultiPoligons
		foreach ( var area in await areas.ToListAsync() )
		{
			var multiPolygon = ToMultiPolygon(area.Boundary);
			var areaXNTS = new ServiceAreaXNTS(area, multiPolygon);

			wrapperAreas.Add(areaXNTS);
		}

		cache.Set(ServiceAreasCacheKey, wrapperAreas, options);
	}

	// ------------------------------------------------------------------------------------------------------<

	private static MultiPolygon ToMultiPolygon(
		GeoJsonMultiPolygon<GeoJson2DGeographicCoordinates> geoJsonMultiPolygon)
	{
		var factory = GeometryFactory.Floating;
		var listPolygons = new List<Polygon>();

		// Convert each geo json poligon into nts compatible polygon
		foreach ( var geoJsonPolygon in geoJsonMultiPolygon.Coordinates.Polygons )
		{
			var shellCoords = geoJsonPolygon
				.Exterior
				.Positions
				.Select(ToNtsCoords)
				.ToArray();

			var holes = geoJsonPolygon
				.Holes
				.Select(
					hole => factory.CreateLinearRing
					(
						[.. hole.Positions.Select(ToNtsCoords)]
					)
				)
				.ToArray();

			// Create shell ring and use it for signle polygon
			var shell = factory.CreateLinearRing(shellCoords);
			var polygon = factory.CreatePolygon(shell, holes);

			listPolygons.Add(polygon);
		}

		return factory.CreateMultiPolygon([..listPolygons]);
	}


	private static Coordinate ToNtsCoords(GeoJson2DGeographicCoordinates coordinates)
	{
		return new Coordinate(coordinates.Longitude, coordinates.Latitude);
	}

	// ------------------------------------------------------------------------------------------------------<
}