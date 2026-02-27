namespace IWKits.Api.Services;

// Namespaces used by this file
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using MongoDB.Driver;
using System;

// Main content of the file
public sealed class GeoLocationService : IGeoLocationService
{
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

	public async Task<ServiceArea?> FindServiceAreaAsync(
		GeoJson2DGeographicCoordinates coordinates)
	{
		var filter = Builders<ServiceAreaFull>.Filter.GeoIntersects(
			x => x.Boundary, GeoJson.Point(coordinates));

		// Try to find service areas withing provided coordinates are defined
		return await coreDatabase.SerAreas
			.Find(filter)
			.As<ServiceArea>()
			.FirstOrDefaultAsync();
	}


	public async Task<GeoZoneInfo?> FindGeoZoneInfoAsync(
		GeoJson2DGeographicCoordinates coordinates, string state)
	{
		// Create geo zone info filter builder
		var filterBuilder = Builders<GeoZoneInfoFull>.Filter;

		// Create filter based on zip code value
		var geoZoneFilter = filterBuilder.And
		(
			filterBuilder.NearSphere(x => x.Coordinates, GeoJson.Point(coordinates)),
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
}