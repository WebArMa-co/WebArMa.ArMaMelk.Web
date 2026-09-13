using WebArMa.ArMaMelk.Web.Application.Profile.DTOs;

namespace WebArMa.ArMaMelk.Web.Application.Profile.Services
{
	public interface IProfileService
	{
		Task<UserProfileDTO?> GetProfileAsync(CancellationToken cancellationToken = default);
		Task<bool> UpdateProfileAsync(UpdateProfileDTO request, CancellationToken cancellationToken = default);
	}
}