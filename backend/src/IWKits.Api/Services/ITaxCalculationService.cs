namespace IWKits.Api;

// Namespaces used by this file
using System.Threading.Tasks;

// Main content of the file
public interface ITaxCalculationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	Task<TaxInfo> CalculateTaxAsync(double latitude, double longitude, decimal subtotal);

	// ------------------------------------------------------------------------------------------------------<
}