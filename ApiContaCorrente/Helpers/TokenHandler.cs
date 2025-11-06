using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiContaCorrente.Helpers
{
    public static class TokenHandler
    {
        public static JwtSecurityToken ReadToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadToken(token) as JwtSecurityToken;
        }
        public static string GetClaimValue(string token, string claimType)
        {
            var jwtToken = ReadToken(token);
            return jwtToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}