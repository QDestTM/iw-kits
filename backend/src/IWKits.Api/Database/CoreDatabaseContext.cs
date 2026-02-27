namespace IWKits.Api.Database;

// Namespaces used by this file
using IWKits.Api.Entities;
using MongoDB.Driver;

// Main content of the file
public sealed class CoreDatabaseContext : DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<TaxRateInfoFull> TaxRates => database.GetCollection<TaxRateInfoFull>("taxrates");

	public IMongoCollection<GeoZoneInfoFull> GeoZones => database.GetCollection<GeoZoneInfoFull>("geozones");

	public IMongoCollection<ServiceAreaFull> SerAreas => database.GetCollection<ServiceAreaFull>("serareas");

	// Public instance constructors
	public CoreDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}

	// ------------------------------------------------------------------------------------------------------<
}