using WebArMa.ArMaMelk.Web.Application.Auth.DTOs;

namespace WebArMa.ArMaMelk.Web.Application.Auth.Services;

public interface IAuthService
{
	Task<TokenDTO?> LoginAsync(string phoneNumber, string code, CancellationToken cancellationToken);
	Task<TokenDTO?> LoginAsync(CancellationToken cancellationToken);
	Task LogoutAsync(bool terminateAllSessions, CancellationToken cancellationToken = default);
}