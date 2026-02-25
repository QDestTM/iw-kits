namespace IWKits.Api;

// Namespaces used by this file
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

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

		// Configure database using manually created scoped lifecycle
		using ( var scope = application.Services.CreateScope() )
		{
			var context = scope.ServiceProvider.GetRequiredService<IMongoDBContext>();

			if ( context is MongoDBContext mongoContext )
			{
				await mongoContext.ConfigureDatabaseAsync();
			}
		}

		// Register endpoints and pipelines
		application.MapApplicationEndpoints();
		application.UseApplicationPipelines();

		// Start the web application
		await application.RunAsync();
	}

	// ------------------------------------------------------------------------------------------------------<

	private static void AddApplicationServices(this WebApplicationBuilder builder)
	{
		var configuration = builder.Configuration;
		var environment   = builder.Environment;
		var services      = builder.Services;

		// Add general services to application
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		// Add database-related services
		services.AddSingleton<IMongoClient>((sp) =>
		{
			var connString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

			// Throw for invalid settings of the environment
			if ( string.IsNullOrWhiteSpace(connString) )
			{
				throw new InvalidOperationException(
					"Critical error: 'DB_CONNECTION_STRING' environment variable is missing. " +
					"Check your .env file or environment settings.");
			}

			return new MongoClient(connectionString: connString);
		});

		services.AddSingleton<IMongoDBContext>((sp) =>
		{
			var databaseName = configuration.GetSection("MongoDatabase:DatabaseName").Value;

			// Throw for invalid configuration of the database name
			if ( string.IsNullOrWhiteSpace(databaseName) )
			{
				throw new InvalidOperationException(
					"Configuration error: 'MongoDatabase:DatabaseName' not found in appsettings.json.");
			}

			var client = sp.GetRequiredService<IMongoClient>();
			return new MongoDBContext(client, databaseName);
		});

		// Add third party API related services
		if ( environment.IsDevelopment() )
		{
			services.AddSingleton<ITaxCalculationService, FakeTaxCalculationService>();
		}
		else
		{
			services.AddScoped<ITaxCalculationService, TaxCalculationService>();
		}
	}


	private static void MapApplicationEndpoints(this WebApplication application)
	{
		var apiV1 = application.MapGroup("api/v1");

		apiV1.MapAddOrderEndpoint();
		apiV1.MapGetOrdersEndpoint();
		apiV1.MapImportOrdersEndpoint();
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