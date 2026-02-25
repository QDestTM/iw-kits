namespace IWKits.Api;

// Namespaces used by this file
using System;

// Main content of the file
public static class Utils
{
	// ^ ----------------------------------------------------------------------------------------------------<

	public static Guid GuidFrom(ulong x)
	{
		Span<byte> bytes = stackalloc byte[16];
		BitConverter.TryWriteBytes(bytes, x);

		return new Guid(bytes);
	}

	// ------------------------------------------------------------------------------------------------------<
}