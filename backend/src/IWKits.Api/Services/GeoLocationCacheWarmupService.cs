namespace IWKits.Api.Services;

// Namespaces used by this file
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

// Main content of the file
public sealed class GeoLocationCacheWarmupService : BackgroundService
{
	private static readonly TimeSpan RefreshTime = TimeSpan.FromHours(1);

	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	private readonly ILogger<GeoLocationCacheWarmupService> logger;
	private readonly IGeoLocationService geoLocation;

	// Public instance constructors
	public GeoLocationCacheWarmupService(IGeoLocationService geoLocation,
		ILogger<GeoLocationCacheWarmupService> logger) : base()
	{
		this.geoLocation = geoLocation;
		this.logger = logger;
	}

	// # ----------------------------------------------------------------------------------------------------<

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while ( !stoppingToken.IsCancellationRequested )
		{
			logger.LogInformation("Refreshing geo location service cache.");

			// Refresh geo location cache and wait for defined amount of time
			await geoLocation.RefreshGeoLocationCache();
			await Task.Delay(RefreshTime, stoppingToken);
		}
	}

	// ------------------------------------------------------------------------------------------------------<
}