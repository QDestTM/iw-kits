namespace IWKits.Api.Services;

// Namespaces used by this file
using IWKits.Api.Entities;

// Main content of the file
public interface ISecurityService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	string GenerateAccessToken(UserInfo userInfo);


	string GenerateRefreshToken();

	// ------------------------------------------------------------------------------------------------------<

	string HashPassword(string password);


	bool VerifyPassword(string hashpass, string password);

	// ------------------------------------------------------------------------------------------------------<
}