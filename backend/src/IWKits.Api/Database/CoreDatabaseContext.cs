namespace IWKits.Api.Database;

// Namespaces used by this file
using IWKits.Api.Entities;
using MongoDB.Driver;

// Main content of the file
public class CoreDatabaseContext : DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<TaxRateInfo> TaxRates => database.GetCollection<TaxRateInfo>("taxrates");

	public IMongoCollection<GeoZipInfo> GeoZips => database.GetCollection<GeoZipInfo>("geozips");

	// Public instance constructors
	public CoreDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}

	// ------------------------------------------------------------------------------------------------------<
}