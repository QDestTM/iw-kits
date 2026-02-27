namespace IWKits.Api.Services;

// Namespaces used by this file
using MongoDB.Driver.GeoJsonObjectModel;
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
	public OrderProcessService(IGeoLocationService geoLocation, ITaxApplierService taxApplier)
	{
		this.geoLocation = geoLocation;
		this.taxApplier = taxApplier;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public async Task<OrderProcessResult> ProcessAsync(RawOrderInfo rawOrder)
	{
		var coordinates = GeoJson.Geographic(rawOrder.Longitude, rawOrder.Latitude);
		var ordId = rawOrder.Id;

		// Find closest service area from the provided coordinates
		var serviceArea = await geoLocation.FindServiceAreaAsync(coordinates);

		if ( serviceArea is null || serviceArea.StateId != "NY" )
		{
			return OrderProcessResult.Failure(
				$"Id({ordId}): Selected location is outside service area.");
		}

		// Calculate tax info to include it into the new order info
		var geoZoneInfo = await geoLocation.FindGeoZoneInfoAsync(coordinates);

		if ( geoZoneInfo is null || geoZoneInfo.StateId != serviceArea.StateId )
		{
			return OrderProcessResult.Failure(
				$"Order {ordId}: Location is in '{geoZoneInfo?.StateId ?? "Unknown state"}'" +
				$" which is outside the required service area '{serviceArea.StateId}'.");
		}

		// Get tax rate info from received geo zip info
		var taxRate = await geoLocation.FindTaxRateInfoAsync(geoZoneInfo.ZipCode, serviceArea.StateId);

		// Tax rate data for this specific ZIP is missing in the database
		if ( taxRate is null )
		{
			return OrderProcessResult.Failure(
				$"Id({ordId}): Tax data is unavailable for the identified area ({geoZoneInfo.ZipCode}).");
		}

		// Calculate applied tax using tax rate, geo zip and subtotal value
		var appliedTax = taxApplier.Apply(taxRate, geoZoneInfo, rawOrder.Subtotal);

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