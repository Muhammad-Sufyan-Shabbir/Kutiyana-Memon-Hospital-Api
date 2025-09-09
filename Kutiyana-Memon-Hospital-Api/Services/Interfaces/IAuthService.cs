using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(AuthRequestDto request);
        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(string email, string token, string newPassword);
    }
}
