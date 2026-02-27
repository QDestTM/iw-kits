namespace IWKits.Api.Services;

// Namespaces used by this file
using IWKits.Api.Entities;

// Main content of the file
public sealed class TaxApplierFakeService : ITaxApplierService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public TaxApplyResult Apply(TaxRateInfo taxRate, GeoZoneInfo geoZip, decimal subtotal)
	{
		// Define fake tax rates
		decimal stateRate = 0.04m;
		decimal cityRate = 0.025m;
		decimal specialRate = 0.015m;

		// Find fake composite rate and calculate amount
		decimal compositeRate = stateRate + cityRate + specialRate;
		decimal taxAmount = System.Math.Round(subtotal * compositeRate, 2);

		// Create tax calculations result
		return new TaxApplyResult()
		{
			CompositeTaxRate = compositeRate,
			TaxAmount = taxAmount,
			TotalAmount = subtotal + taxAmount,

			Breakdown = new TaxBreakdown
			{
				StateRate = stateRate,
				CityRate = cityRate,
				SpecialRate = specialRate,
				CountyRate = 0.0m
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