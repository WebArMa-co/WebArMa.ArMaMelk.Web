using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using WebArMa.ArMaMelk.Web.Application.Users.DTOs;
using WebArMa.ArMaMelk.Web.Application.Users.ViewModels;

namespace WebArMa.ArMaMelk.Web.Application.Users.Services
{
	public class UserService(IHttpContextAccessor httpContextAccessor,HttpClient httpClient) : IUserService
	{
		public async Task<UserDTO?> GetUserAsync(CancellationToken cancellationToken = default)
		{
			var response = await httpClient.GetAsync($"api/v1/User/{httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.NameId)}", cancellationToken);
			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadFromJsonAsync<UserDTO>(cancellationToken: cancellationToken);
		}

		public async Task<bool> UpdateUserAsync(UpdateUserViewModel request, CancellationToken cancellationToken = default)
		{
			var response = await httpClient.PutAsJsonAsync("api/v1/User", request, cancellationToken);
			return response.IsSuccessStatusCode;
		}
	}
}