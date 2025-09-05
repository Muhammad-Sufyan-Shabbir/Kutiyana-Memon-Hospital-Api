using Kutiyana_Memon_Hospital_Api.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kutiyana_Memon_Hospital_Api.API.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(User user, string key, string issuer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "User"),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(token);

        }
    }
}
