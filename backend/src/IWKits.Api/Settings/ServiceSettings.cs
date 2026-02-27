namespace IWKits.Api.Settings;

// Namespaces used by this file
using System.ComponentModel.DataAnnotations;

// Main content of the file
public sealed class ServiceSettings
{
	public const string SectionName = "Services";

	// ^ ----------------------------------------------------------------------------------------------------<

	[Required, RegularExpression("default|fake")]
	public string TaxApplier { get; set; } = string.Empty;

	[Required, RegularExpression("default|fake")]
	public string OrderProcess { get; set; } = string.Empty;

	[Required, RegularExpression("default|fake")]
	public string GeoLocation { get; set; } = string.Empty;

	// ------------------------------------------------------------------------------------------------------<
}