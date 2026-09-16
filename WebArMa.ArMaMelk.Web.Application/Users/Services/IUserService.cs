using WebArMa.ArMaMelk.Web.Application.Users.DTOs;
using WebArMa.ArMaMelk.Web.Application.Users.ViewModels;

namespace WebArMa.ArMaMelk.Web.Application.Users.Services
{
	public interface IUserService
	{
		Task<UserDTO?> GetUserAsync(CancellationToken cancellationToken = default);
		Task<bool> UpdateUserAsync(UpdateUserViewModel request, CancellationToken cancellationToken = default);
	}
}