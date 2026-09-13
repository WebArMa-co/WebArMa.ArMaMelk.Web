namespace WebArMa.ArMaMelk.Web.Application.Profile.DTOs
{
	public class UpdateProfileDTO
	{
		public string FullName { get; set; } = string.Empty;
		public string? AgencyName { get; set; }
		public string? Email { get; set; }
	}
}