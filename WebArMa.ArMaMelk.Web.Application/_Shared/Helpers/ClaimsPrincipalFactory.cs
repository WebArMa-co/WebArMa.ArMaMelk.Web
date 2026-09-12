using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebArMa.ArMaMelk.Web.Application._Shared.Helpers
{
    public static class ClaimsPrincipalFactory
    {
        public static ClaimsPrincipal Create(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            var identity = new ClaimsIdentity(jwt.Claims, authenticationType: "ArMaMelk", nameType: ClaimTypes.Name, roleType: ClaimTypes.Role);
            return new ClaimsPrincipal(identity);
        }
    }
}
