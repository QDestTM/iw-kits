namespace IWKits.Api.Services;

// Namespaces used by this file
using MongoDB.Driver.GeoJsonObjectModel;
using System.Threading.Tasks;
using IWKits.Api.Entities;

// Main content of the file
public interface IGeoLocationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	Task<ServiceArea?> FindServiceAreaAsync(GeoJson2DGeographicCoordinates coordinates);


	Task<GeoZoneInfo?> FindGeoZoneInfoAsync(GeoJson2DGeographicCoordinates coordinates);


	Task<TaxRateInfo?> FindTaxRateInfoAsync(int zipCode, string state);

	// ------------------------------------------------------------------------------------------------------<
}