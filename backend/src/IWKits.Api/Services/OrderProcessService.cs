namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Threading.Tasks;
using IWKits.Api.Entities;
using System;

// Main content of the file
public sealed class OrderProcessService : IOrderProcessService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private instance members
	private readonly IGeoLocationService geoLocation;
	private readonly ITaxApplierService taxApplier;

	// Public instance constructors
	public OrderProcessService(
		IGeoLocationService geoLocation, ITaxApplierService taxApplier)
	{
		this.geoLocation = geoLocation;
		this.taxApplier = taxApplier;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<OrderProcessResult> ProcessAsync(RawOrderInfo rawOrder)
	{
		// Calculate tax info to include it into the new order info
		var nearGeo = await geoLocation.FindNearestZoneAsync
		(
			lng: rawOrder.Longitude,
			lat: rawOrder.Latitude
		);

		// Not in service area if geo is null or state is not NY
		if ( nearGeo is null || nearGeo.StateId != "NY" )
		{
			return OrderProcessResult.Failure(
				$"Id({rawOrder.Id}): Selected location is outside service area.");
		}

		// Get tax rate info from received geo zip info
		var taxRate = await geoLocation.GetTaxRateAsync(nearGeo.ZipCode);

		// Tax rate data for this specific ZIP is missing in the database
		if ( taxRate is null )
		{
			return OrderProcessResult.Failure(
				$"Id({rawOrder.Id}): Tax data is unavailable for the identified area ({nearGeo.ZipCode}).");
		}

		// Calculate applied tax using tax rate, geo zip and subtotal value
		var appliedTax = taxApplier.Apply(taxRate, nearGeo, rawOrder.Subtotal);

		// Return failure result if applied tax has errors
		if ( appliedTax.HasError )
		{
			return OrderProcessResult.Failure(appliedTax.ErrorMessage);
		}

		// Create success result from tax info and raw order
		return OrderProcessResult.Success(new()
		{
			Id = Guid.NewGuid(),

			Latitude  = rawOrder.Latitude,
			Longitude = rawOrder.Longitude,
			Subtotal  = rawOrder.Subtotal,

			CompositeTaxRate = appliedTax.CompositeTaxRate,
			TaxAmount        = appliedTax.TaxAmount,
			TotalAmount      = appliedTax.TotalAmount,
			Breakdown        = appliedTax.Breakdown,
			Jurisdictions    = appliedTax.Jurisdictions,

			Timestamp = rawOrder.Timestamp
		});
	}

	// ------------------------------------------------------------------------------------------------------<
}