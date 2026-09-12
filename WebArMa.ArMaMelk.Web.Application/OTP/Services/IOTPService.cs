namespace WebArMa.ArMaMelk.Web.Application.OTP.Services
{
	public interface IOTPService
	{
		Task RequestOTPAsync(string phoneNumber, CancellationToken cancellationToken);
	}
}
