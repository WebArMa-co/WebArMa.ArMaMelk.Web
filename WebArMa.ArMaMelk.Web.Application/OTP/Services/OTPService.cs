using System.Net.Http.Json;

namespace WebArMa.ArMaMelk.Web.Application.OTP.Services
{
	public class OTPService(HttpClient httpClient) : IOTPService
	{
		public async Task RequestOTPAsync(string phoneNumber, CancellationToken cancellationToken)
		{
			var request = new
			{
				phoneNumber
			};

			await httpClient.PostAsJsonAsync("api/v1/OTP/Request", request, cancellationToken);
		}
	}
}
