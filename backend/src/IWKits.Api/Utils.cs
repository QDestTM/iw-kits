namespace IWKits.Api;

// Namespaces used by this file
using System;

// Main content of the file
public static class Utils
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public static string GetRequiredEnv(string key)
	{
		string? value = Environment.GetEnvironmentVariable(key);
		if ( value is not null ) return value;

		// Throw for invalid settings of the environment
		throw new InvalidOperationException
		(
			$"Critical error: '{key}' environment variable is missing. " +
			"Check your .env file or environment settings."
		);
	}

	// ------------------------------------------------------------------------------------------------------<
}