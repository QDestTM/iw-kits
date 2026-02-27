namespace IWKits.Api.Entities;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System;

// Main content of the file
public sealed class RawOrderInfo
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[JsonPropertyName("id")]
	[CsvHelper.Configuration.Attributes.Name("id")]
	public ulong Id { get; set; }

	[JsonPropertyName("latitude")]
	[CsvHelper.Configuration.Attributes.Name("latitude")]
	public double Latitude { get; set; }

	[JsonPropertyName("longitude")]
	[CsvHelper.Configuration.Attributes.Name("longitude")]
	public double Longitude { get; set; }

	[JsonPropertyName("subtotal")]
	[CsvHelper.Configuration.Attributes.Name("subtotal")]
	public decimal Subtotal { get; set; }

	[JsonPropertyName("timestamp")]
	[CsvHelper.Configuration.Attributes.Name("timestamp")]
	public DateTime Timestamp { get; set; }

	// ------------------------------------------------------------------------------------------------------<
}