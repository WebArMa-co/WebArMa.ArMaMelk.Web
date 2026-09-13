using System.Net.Http.Json;
using WebArMa.ArMaMelk.Web.Application.Profile.DTOs;

namespace WebArMa.ArMaMelk.Web.Application.Profile.Services
{
	public class ProfileService(HttpClient httpClient) : IProfileService
	{
		public async Task<UserProfileDTO?> GetProfileAsync(CancellationToken cancellationToken = default)
		{
			var response = await httpClient.GetAsync("api/v1/Persons/CreatePerson", cancellationToken);
			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadFromJsonAsync<UserProfileDTO>(cancellationToken: cancellationToken);
		}

		public async Task<bool> UpdateProfileAsync(UpdateProfileDTO request, CancellationToken cancellationToken = default)
		{
			var response = await httpClient.PutAsJsonAsync("api/v1/Persons/UpdatePerson", request, cancellationToken);
			return response.IsSuccessStatusCode;
		}
	}
}