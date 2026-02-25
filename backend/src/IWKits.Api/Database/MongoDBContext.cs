namespace IWKits.Api;

// Namespaces used by this file
using System.Threading.Tasks;
using MongoDB.Driver;

// Main content of the file
public sealed class MongoDBContext : IMongoDBContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<OrderInfo> Orders => database.GetCollection<OrderInfo>("orders");

	public IMongoCollection<UserInfo>  Users  => database.GetCollection<UserInfo>  ("users");

	//! Private instance members
	private readonly IMongoDatabase database;

	// Public instance constructors
	public MongoDBContext(IMongoClient client, string databaseName)
	{
		database = client.GetDatabase(databaseName);
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task ConfigureDatabaseAsync()
	{
		
	}

	// ------------------------------------------------------------------------------------------------------<
}