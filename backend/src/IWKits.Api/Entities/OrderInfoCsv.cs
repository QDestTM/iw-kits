namespace IWKits.Api;

// Namespaces used by this file
using System.Text.Json.Serialization;
using System;

// Main content of the file
public sealed record OrderInfoCsv
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[JsonPropertyName("id")]
	[CsvHelper.Configuration.Attributes.Name("id")]
	public ulong Id { get; init; }

	[JsonPropertyName("latitude")]
	[CsvHelper.Configuration.Attributes.Name("latitude")]
	public double Latitude { get; init; }

	[JsonPropertyName("longitude")]
	[CsvHelper.Configuration.Attributes.Name("longitude")]
	public double Longitude { get; init; }

	[JsonPropertyName("subtotal")]
	[CsvHelper.Configuration.Attributes.Name("subtotal")]
	public decimal Subtotal { get; init; }

	[JsonPropertyName("timestamp")]
	[CsvHelper.Configuration.Attributes.Name("timestamp")]
	public DateTime Timestamp { get; init; }

	// ------------------------------------------------------------------------------------------------------<
}