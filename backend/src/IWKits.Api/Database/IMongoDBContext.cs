namespace IWKits.Api;

// Namespaces used by this file
using MongoDB.Driver;

// Main content of the file
public interface IMongoDBContext
{
	// ^ ----------------------------------------------------------------------------------------------------<

	IMongoCollection<OrderInfo> Orders { get; }

	IMongoCollection<UserInfo>  Users  { get; }

	// ------------------------------------------------------------------------------------------------------<
}