namespace IWKits.Api.Settings;

// Namespaces used by this file
using System.ComponentModel.DataAnnotations;

// Main content of the file
public sealed class DatabasesNameSettings
{
	// ^ ----------------------------------------------------------------------------------------------------<

	[Required, MinLength(5)]
	public string Data { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string Auth { get; set; } = string.Empty;

	[Required, MinLength(5)]
	public string Core { get; set; } = string.Empty;

	// ------------------------------------------------------------------------------------------------------<
}