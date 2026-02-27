namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Threading.Tasks;
using IWKits.Api.Entities;

// Main content of the file
public interface IGeoLocationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	Task<GeoZoneInfo?> FindNearestZoneAsync(double lng, double lat);


	Task<TaxRateInfo?> GetTaxRateAsync(int zipCode);

	// ------------------------------------------------------------------------------------------------------<
}