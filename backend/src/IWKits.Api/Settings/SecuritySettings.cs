namespace IWKits.Api.Settings;

// Namespaces used by this file
using System.ComponentModel.DataAnnotations;

// Main content of the file
public sealed class SecuritySettings
{
	public const string SectionName = "Security";

	// ^ ----------------------------------------------------------------------------------------------------<

	[Required, MinLength(5)]
	public string JwtIssuer { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string JwtAudience { get; set; } = string.Empty;

	// ------------------------------------------------------------------------------------------------------<
}