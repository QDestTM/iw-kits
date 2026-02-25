namespace IWKits.Api;

// Namespaces used by this file
using System.Threading.Tasks;

// Main content of the file
public sealed class TaxCalculationService : ITaxCalculationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<TaxInfo> CalculateTaxAsync(
		double latitude, double longitude, decimal subtotal)
	{
		return new TaxInfo();
	}

	// ------------------------------------------------------------------------------------------------------<
}