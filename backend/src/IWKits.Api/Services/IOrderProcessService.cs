namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Threading.Tasks;
using IWKits.Api.Entities;

// Main content of the file
public interface IOrderProcessService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	Task<OrderProcessResult> ProcessAsync(RawOrderInfo rawOrder);

	// ------------------------------------------------------------------------------------------------------<
}