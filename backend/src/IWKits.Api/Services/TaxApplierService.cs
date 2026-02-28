namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Collections.Generic;
using IWKits.Api.Entities;
using System;

// Main content of the file
public sealed class TaxApplierService : ITaxApplierService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public TaxApplyResult Apply(TaxRateInfo taxRate, GeoZoneInfo geoZone, decimal subtotal)
	{
		if ( taxRate.ZipCode != geoZone.ZipCode )
		{
			return TaxApplyResult.Failure(
				$"Zip codes is not match: {nameof(TaxRateInfo)}({taxRate.ZipCode})" +
				$"and {nameof(GeoZoneInfoFull)}({geoZone.ZipCode})");
		}

		// Create breakdown from the estimated tax rates
		var breakdown = new TaxBreakdown
		{
			StateRate   = taxRate.StateRate,
			CountyRate  = taxRate.EstimatedCountyRate,
			CityRate    = taxRate.EstimatedCityRate,
			SpecialRate = taxRate.EstimatedSpecialRate
		};

		// Get estimated combined rate for futher calculations
		decimal compositeRate = taxRate.EstimatedCombinedRate;

		// Perform tax calculation to determine tax amount and total amount
		decimal taxAmount = Math.Round(subtotal * compositeRate, 2, MidpointRounding.AwayFromZero);
		decimal totalAmount = subtotal + taxAmount;

		// Create list of jurisdictions by using names in geo zip info
		var jurisdictions = new List<TaxJurisdiction>(capacity: 4);

		AddIfHasRate(jurisdictions, breakdown.StateRate,   "state",   geoZone.StateName  );
		AddIfHasRate(jurisdictions, breakdown.CountyRate,  "county",  geoZone.CountyName );
		AddIfHasRate(jurisdictions, breakdown.CityRate,    "city",    geoZone.CityName   );
		AddIfHasRate(jurisdictions, breakdown.SpecialRate, "special", "Special District");

		// Combine all calculated data into tax info
		return new TaxApplyResult()
		{
			CompositeTaxRate = compositeRate,
			TaxAmount        = taxAmount,
			TotalAmount      = totalAmount,
			Breakdown        = breakdown,
			Jurisdictions    = jurisdictions
		};
	}

	// ------------------------------------------------------------------------------------------------------<

	private static void AddIfHasRate(List<TaxJurisdiction> jurs, decimal rate, string type, string name)
	{
		if ( rate > 0 )
		{
			jurs.Add(new() { Name = name, Type = type, Rate = rate});
		}
	}

	// ------------------------------------------------------------------------------------------------------<
}