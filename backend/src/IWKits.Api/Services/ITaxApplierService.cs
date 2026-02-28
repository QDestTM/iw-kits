namespace IWKits.Api.Services;

// Namespaces used by this file
using IWKits.Api.Entities;

// Main content of the file
public interface ITaxApplierService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	TaxApplyResult Apply(TaxRateInfo taxRate, GeoZoneInfo geoZone, decimal subtotal);

	// ------------------------------------------------------------------------------------------------------<
}