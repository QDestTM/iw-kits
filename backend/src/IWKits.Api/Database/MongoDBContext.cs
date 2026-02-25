namespace IWKits.Api;

// Namespaces used by this file
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using System;

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
		await ConfigureOrdersCollectionIndexesAsync();
	}

	// ------------------------------------------------------------------------------------------------------<

	private async Task ConfigureOrdersCollectionIndexesAsync()
	{
		var indexKeysDefBuilder = Builders<OrderInfo>.IndexKeys;

		// Define the list of fields that will support sorting
		Expression<Func<OrderInfo, object>>[] fieldsToIndex =
		[
			x => x.Timestamp,
			x => x.CompositeTaxRate,
			x => x.Subtotal,
			x => x.TaxAmount,
			x => x.TotalAmount
		];

		// Project each field into a Compound Index model
		var indexDefinitions = fieldsToIndex.Select((field) =>
		{
			var keys = indexKeysDefBuilder.Combine
			(
				indexKeysDefBuilder.Descending(field),
				indexKeysDefBuilder.Ascending(x => x.Id)
			);

			return new CreateIndexModel<OrderInfo>(keys);
		});

		// Execute async index creation with created index definitions
		await Orders.Indexes.CreateManyAsync(indexDefinitions);
	}

	// ------------------------------------------------------------------------------------------------------<
}