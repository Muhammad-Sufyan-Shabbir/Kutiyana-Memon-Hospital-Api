using BCrypt.Net;
using Kutiyana_Memon_Hospital_Api.API.Helpers;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto dto)
        {
            var user = await _uow.Users.GetByEmailOrUserNameAsync(dto.Identifier);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var role = (await _uow.Roles.GetByIdAsync(user.RoleId))?.Name ?? "User";

            var token = JwtHelper.GenerateToken(user, _config["Jwt:Key"], _config["Jwt:Issuer"]);

            return new AuthResponseDto
            {

                Token = token,
                ImageUrl = user.ImageUrl,
                FullName = user.FullName,
                Role = role,
                Label = user.Label,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}
