namespace IWKits.Api.Database;

// Namespaces used by this file
using IWKits.Api.Entities;
using MongoDB.Driver;

// Main content of the file
public sealed class CoreDatabaseContext : DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<TaxRateInfo> TaxRates => database.GetCollection<TaxRateInfo>("taxrates");

	public IMongoCollection<GeoZoneInfo> GeoZones => database.GetCollection<GeoZoneInfo>("geozones");

	public IMongoCollection<ServiceArea> SerAreas => database.GetCollection<ServiceArea>("serareas");

	// Public instance constructors
	public CoreDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}

	// ------------------------------------------------------------------------------------------------------<
}