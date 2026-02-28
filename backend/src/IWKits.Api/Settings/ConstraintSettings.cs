namespace IWKits.Api.Settings;

// Namespaces used by this file
using System.ComponentModel.DataAnnotations;

// Main content of the file
public sealed class ConstraintSettings
{
	public const string SectionName = "Constraints";

	// ^ ----------------------------------------------------------------------------------------------------<

	[Required, Range(8, 256)]
	public int ImportChunkSize { get; set; } = 0;

	[Required, Range(16, int.MaxValue)]
	public int RespondMaxPageSize { get; set; } = 0;

	// ------------------------------------------------------------------------------------------------------<
}