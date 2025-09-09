using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, Role role, List<RoleModuleAccess> moduleAccesses)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("RoleId", role.Id.ToString()),
                new Claim("RoleName", role.Name ?? ""),
                new Claim("CompanyId", user.CompanyId.ToString()),
                new Claim("CompanyName", user.company?.Name ?? "")
            };
             
            var permissions = new Dictionary<string, object>();

            foreach (var access in moduleAccesses)
            {
                if (access.Module == null) continue;

                var moduleName = access.Module.Name;  

                permissions[moduleName] = new
                {
                    Create = access.CanCreate,
                    Read = access.CanRead,
                    Update = access.CanUpdate,
                    Delete = access.CanDelete
                };
            }

            var permissionsJson = System.Text.Json.JsonSerializer.Serialize(permissions);

            claims.Add(new Claim("permissions", permissionsJson));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
