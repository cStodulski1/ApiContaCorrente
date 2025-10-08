using ApiContaCorrente.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ApiContaCorrente.Authentication
{
    internal sealed class TokenProvider(IConfiguration configuration)
    {
        public string Create (ContaCorrente contaCorrente)
        {
            string secretKey = configuration["Jwt:Secret"]; 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, contaCorrente.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, contaCorrente.Nome),
                    new Claim("cpf", contaCorrente.Cpf)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
