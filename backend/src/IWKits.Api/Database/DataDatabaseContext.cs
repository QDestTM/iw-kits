namespace IWKits.Api.Database;

// Namespaces used by this file
using IWKits.Api.Entities;
using MongoDB.Driver;

// Main content of the file
public class DataDatabaseContext : DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<OrderInfo> Orders => database.GetCollection<OrderInfo>("orders");

	// Public instance constructors
	public DataDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}

	// ------------------------------------------------------------------------------------------------------<
}