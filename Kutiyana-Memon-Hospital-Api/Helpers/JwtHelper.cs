using Kutiyana_Memon_Hospital_Api.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kutiyana_Memon_Hospital_Api.API.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(User user, Role role, List<RoleModuleAccess> moduleAccesses, string key, string issuer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("FullName", user.FullName ?? ""),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim("RoleName", role.Name ?? "User"),
            new Claim("CompanyId", user.CompanyId.ToString())
        };

            // 🔹 Add Module permissions with Module.Name instead of ModuleId
            foreach (var access in moduleAccesses)
            {
                if (access.Module == null) continue;

                var moduleName = access.Module.Name;

                if (access.CanCreate) claims.Add(new Claim($"{moduleName}:Create", "true"));
                if (access.CanRead) claims.Add(new Claim($"{moduleName}:Read", "true"));
                if (access.CanUpdate) claims.Add(new Claim($"{moduleName}:Update", "true"));
                if (access.CanDelete) claims.Add(new Claim($"{moduleName}:Delete", "true"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
