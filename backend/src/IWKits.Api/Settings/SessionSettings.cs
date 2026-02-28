namespace IWKits.Api.Settings;

// Namespaces used by this file
using System.ComponentModel.DataAnnotations;

// Main content of the file
public sealed class SessionSettings
{
	public const string SectionName = "Session";

	// ^ ----------------------------------------------------------------------------------------------------<

	[Required, Range(1, int.MaxValue)]
	public int AccessPeriod { get; set; } = int.MinValue;

	[Required, Range(1, int.MaxValue)]
	public int RefreshPeriod { get; set; } = int.MaxValue;

	// ------------------------------------------------------------------------------------------------------<
}