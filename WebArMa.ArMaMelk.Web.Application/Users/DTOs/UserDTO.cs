using WebArMa.ArMaMelk.Web.Application._Shared.DTOs;

namespace WebArMa.ArMaMelk.Web.Application.Users.DTOs
{
	public class UserDTO : DTOBase
	{
		public string UserName { get; set; } = string.Empty;
		public string? DisplayName { get; set; }
		public string? PhotoUrl { get; set; }
		public string EffectiveDisplayName => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : UserName;
	}
}