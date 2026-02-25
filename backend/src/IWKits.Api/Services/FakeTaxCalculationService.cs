namespace IWKits.Api;

// Namespaces used by this file
using System.Threading.Tasks;

// Main content of the file
public sealed class FakeTaxCalculationService : ITaxCalculationService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public async Task<TaxInfo> CalculateTaxAsync(
		double latitude, double longitude, decimal subtotal)
	{
		await Task.Delay(50); // Imitate API latency

		// Define fake tax rates
		decimal stateRate = 0.04m;
		decimal cityRate = 0.025m;
		decimal specialRate = 0.015m;

		// Find fake composite rate and calculate amount
		decimal compositeRate = stateRate + cityRate + specialRate;
		decimal taxAmount = System.Math.Round(subtotal * compositeRate, 2);

		// Finaly create tax calculation result
		return new TaxInfo
		{
			CompositeTaxRate = compositeRate,
			TaxAmount = taxAmount,
			TotalAmount = subtotal + taxAmount,

			Breakdown = new TaxBreakdown
			{
				StateRate = stateRate,
				CityRate = cityRate,
				SpecialRate = specialRate,
				CountryRate = 0.0m
			},

			Jurisdictions =
			[
				new TaxJurisdiction()
				{
					Name = "California",
					Type = "state",
					Rate = stateRate
				},

				new TaxJurisdiction()
				{
					Name = "Los Angeles",
					Type = "city",
					Rate = cityRate
				},

				new TaxJurisdiction()
				{
					Name = "Transit District",
					Type = "special",
					Rate = specialRate
				}
			]
		};
	}

	// ------------------------------------------------------------------------------------------------------<
}