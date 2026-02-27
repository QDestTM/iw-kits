namespace IWKits.Api.Services;

// Namespaces used by this file
using MongoDB.Driver.GeoJsonObjectModel;
using System.Threading.Tasks;
using IWKits.Api.Entities;
using IWKits.Api.Database;
using MongoDB.Driver;

// Main content of the file
public sealed class GeoLocationService : IGeoLocationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	private readonly CoreDatabaseContext coreDatabase;

	// Public instance constructors
	public GeoLocationService(CoreDatabaseContext coreDatabase)
	{
		this.coreDatabase = coreDatabase;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<GeoZoneInfo?> FindNearestZoneAsync(double lng, double lat)
	{
		var filterBuilder = Builders<GeoZoneInfo>.Filter;
		var coordinates = GeoJson.Geographic(lng, lat);

		// Create filter based on GeoJson position
		var nearSphereFilter = filterBuilder.NearSphere(
			x => x.Coordinates, GeoJson.Point(coordinates));

		// Use created filter to find first suitable element
		return await coreDatabase.GeoZones
			.Find(nearSphereFilter)
			.Limit(limit: 1)
			.FirstOrDefaultAsync();
	}


	public async Task<TaxRateInfo?> GetTaxRateAsync(int zipCode)
	{
		var filterBuilder = Builders<TaxRateInfo>.Filter;

		// Create filter based on zip code value
		var zipCodeFilter = filterBuilder.Eq(x => x.ZipCode, zipCode);

		// Use created filter to find first suitable element
		return await coreDatabase.TaxRates
			.Find(zipCodeFilter)
			.Limit(limit: 1)
			.FirstOrDefaultAsync();
	}

	// ------------------------------------------------------------------------------------------------------<
}