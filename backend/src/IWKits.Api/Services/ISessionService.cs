namespace IWKits.Api.Services;

// Namespaces used by this file
using System.Threading.Tasks;
using IWKits.Api.Entities;
using System.Threading;

// Main content of the file
public interface ISessionService
{
	// ^ ----------------------------------------------------------------------------------------------------<

	Task<CreateSessionResult> CreateSessionAsync(UserInfo user, CancellationToken ct);


	Task<RefreshSessionResult> RefreshSessionAsync(string refreshToken, CancellationToken ct);

	// ------------------------------------------------------------------------------------------------------<
}