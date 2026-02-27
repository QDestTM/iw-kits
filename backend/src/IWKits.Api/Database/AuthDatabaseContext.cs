namespace IWKits.Api.Database;

// Namespaces used by this file
using IWKits.Api.Entities;
using MongoDB.Driver;

// Main content of the file
public sealed class AuthDatabaseContext : DatabaseContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// Public instance properties
	public IMongoCollection<UserInfo> Users => database.GetCollection<UserInfo>("users");

	// Public instance constructors
	public AuthDatabaseContext(IMongoClient client, string databaseName)
		: base(client, databaseName) {}

	// ------------------------------------------------------------------------------------------------------<
}