namespace IWKits.Api.Database;

// Namespaces used by this file
using MongoDB.Driver;

// Main content of the file
public abstract class DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	protected readonly IMongoDatabase database;

	// Public instance constructors
	public DatabaseContext(IMongoClient client, string databaseName)
	{
		database = client.GetDatabase(databaseName);
	}

	// ------------------------------------------------------------------------------------------------------<
}