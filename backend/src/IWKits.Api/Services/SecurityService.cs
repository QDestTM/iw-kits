namespace IWKits.Api.Services;

// Namespaces used by this file
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using IWKits.Api.Entities;
using IWKits.Api.Settings;
using System.Text;
using BCrypt.Net;
using System;

// Main content of the file
public sealed class SecurityService : ISecurityService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	//! Private members
	private readonly SecuritySettings securitySettings;
	private readonly SessionSettings sessionSettings;
	private readonly string jwtKey;

	// Public instance constructors
	public SecurityService(SecuritySettings securitySettings,
		SessionSettings sessionSettings, string jwtKey)
	{
		this.securitySettings = securitySettings;
		this.sessionSettings = sessionSettings;
		this.jwtKey = jwtKey;
	}

	// # ----------------------------------------------------------------------------------------------------<

	public string GenerateAccessToken(UserInfo userInfo)
	{
		var securityKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(jwtKey) );
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		// Find expiration date by adding AccessPeriod minutes to the current datetime
		var expirationDate = DateTime.UtcNow.AddMinutes(sessionSettings.AccessPeriod);

		// Create new jwt security token using credentials and user info
		var token = new JwtSecurityToken(
			issuer  : securitySettings.JwtIssuer,
			audience: securitySettings.JwtAudience,
			expires : expirationDate,

			signingCredentials: credentials,
			claims:
			[
				new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
				new Claim(ClaimTypes.Name,           userInfo.Username),
				new Claim(ClaimTypes.Role,           userInfo.Role)
			]
		);

		// Convert created token into compact serialization format
		return new JwtSecurityTokenHandler().WriteToken(token);
	}


	public string GenerateRefreshToken()
	{
		return Convert.ToBase64String( RandomNumberGenerator.GetBytes(32) );
	}

	// ------------------------------------------------------------------------------------------------------<

	public string HashPassword(string text)
	{
		return BCrypt.HashPassword(text);
	}


	public bool VerifyPassword(string hash, string text)
	{
		return BCrypt.Verify(text, hash);
	}

	// ------------------------------------------------------------------------------------------------------<
}