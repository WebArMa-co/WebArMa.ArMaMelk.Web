using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebArMa.ArMaMelk.Web.Application.Auth.DTOs;

namespace WebArMa.ArMaMelk.Web.Application._Shared.Delegates
{

    public sealed class ApiAuthenticationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken, allowRefresh: true);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, bool allowRefresh)
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var accessToken = await httpContext.GetTokenAsync("ArMaMelk", "access_token");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            if (!allowRefresh)
            {
                return response;
            }

            response.Dispose();

            var refreshToken = await httpContext.GetTokenAsync("ArMaMelk", "refresh_token");

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return response;
            }

            var refreshed = await RefreshTokenAsync(refreshToken, cancellationToken);

            if (!refreshed)
            {
                return response;
            }

            return await SendAsync(request, cancellationToken, allowRefresh: false);
        }

        private static async Task<bool> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            var response = await client.PostAsJsonAsync("Auth/RefreshLogin", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<TokenDTO>(cancellationToken);

            if (result is null)
            {
                return false;
            }

            return true;
        }
    }
}
