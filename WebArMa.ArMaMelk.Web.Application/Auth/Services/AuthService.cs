using System.Net.Http.Json;
using WebArMa.ArMaMelk.Web.Application.Auth.DTOs;

namespace WebArMa.ArMaMelk.Web.Application.Auth.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
	public async Task<TokenDTO?> LoginAsync(string phoneNumber, string code, CancellationToken cancellationToken = default)
	{

		var request = new
		{
			UserName = phoneNumber,
			Code = code
		};

		var response = await httpClient.PostAsJsonAsync("api/v1/Auth/Login", request, cancellationToken);
		var token = await response.Content.ReadFromJsonAsync<TokenDTO>(cancellationToken);
		return token;
	}
}