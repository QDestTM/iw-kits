namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using IWKits.Api.Settings;
using IWKits.Api.Database;
using IWKits.Api.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Caching.Memory;

// Main content of the file
public static class IWKitsApi
{
	// ^ ----------------------------------------------------------------------------------------------------<

	private static async Task Main()
	{
		var builder = WebApplication.CreateBuilder();

		// Register services and build application
		builder.AddApplicationServices();
		var application = builder.Build();

		// Register endpoints and pipelines
		application.MapApplicationEndpoints();
		application.UseApplicationPipelines();

		// Start the web application
		await application.RunAsync();
	}

	// ------------------------------------------------------------------------------------------------------<

	private static void AddApplicationServices(this WebApplicationBuilder builder)
	{
		var cfg = builder.Configuration;
		var env = builder.Environment;
		var srv = builder.Services;

		// Add general services to application
		srv.AddControllers();
		srv.AddEndpointsApiExplorer();
		srv.AddMemoryCache();
		srv.AddSwaggerGen();

		// Add options-related services
		srv.AddOptions<MongoDBSettings>()
			.Bind( cfg.GetSection(MongoDBSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<ConstraintSettings>()
			.Bind( cfg.GetSection(ConstraintSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		srv.AddOptions<ServiceSettings>()
			.Bind( cfg.GetSection(ServiceSettings.SectionName) )
			.ValidateDataAnnotations()
			.ValidateOnStart();

		// Add database-related services
		srv.AddSingleton<IMongoClient>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			string username = Utils.GetRequiredEnv("DB_CONNECTION_USERNAME");
			string password = Utils.GetRequiredEnv("DB_CONNECTION_PASSWORD");

			string hostname = Utils.GetRequiredEnv("DB_CONNECTION_HOSTNAME");
			string hostport = Utils.GetRequiredEnv("DB_CONNECTION_HOSTPORT");

			// Create connection string with username, password and hostname variables
			return new MongoClient(
				$"mongodb://{username}:{password}@{hostname}:{hostport}/?authSource={settings.AuthSource}"
			);
		});

		srv.AddSingleton<AuthDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			// Get running mongo client and create context
			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Auth);
		});

		srv.AddSingleton<CoreDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			// Get running mongo client and create context
			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Core);
		});

		srv.AddSingleton<DataDatabaseContext>((sp) =>
		{
			var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;

			// Get running mongo client and create context
			var client = sp.GetRequiredService<IMongoClient>();
			return new(client, settings.Databases.Data);
		});

		// Add other singleton services
		srv.AddSingleton<ITaxApplierService>((sr) =>
		{
			var settings = sr.GetRequiredService<IOptions<ServiceSettings>>().Value;

			return ( settings.TaxApplier == "fake" ) ? new TaxApplierFakeService()
				: new TaxApplierService();
		});

		srv.AddSingleton<IGeoLocationService>((sr) =>
		{
			var coreDatabase = sr.GetRequiredService<CoreDatabaseContext>();
			var memoryCache = sr.GetRequiredService<IMemoryCache>();

			return new GeoLocationService(coreDatabase, memoryCache);
		});

		srv.AddSingleton<IOrderProcessService>((sr) =>
		{
			var geoLocation = sr.GetRequiredService<IGeoLocationService>();
			var taxApplier = sr.GetRequiredService<ITaxApplierService>();

			return new OrderProcessService(geoLocation, taxApplier);
		});
	}


	private static void MapApplicationEndpoints(this WebApplication application)
	{
		var apiV1 = application.MapGroup("api/v1");

		Features.CreateOrder.CreateOrderEndpoint.MapCreateOrderEndpoint(apiV1);
		Features.GetOrders.GetOrdersEndpoint.MapGetOrdersEndpoint(apiV1);
		Features.ImportOrders.ImportOrdersEndpoint.MapImportOrdersEndpoint(apiV1);
	}


	private static void UseApplicationPipelines(this WebApplication application)
	{
		var environment = application.Environment;

		// Register development-only pipelanes
		if ( environment.IsDevelopment() )
		{
			application.UseSwaggerUI();
			application.UseSwagger();
		}

		// Register production-related pipelines
		application.UseHttpsRedirection();
		application.UseAuthorization();
	}

	// ------------------------------------------------------------------------------------------------------<
}