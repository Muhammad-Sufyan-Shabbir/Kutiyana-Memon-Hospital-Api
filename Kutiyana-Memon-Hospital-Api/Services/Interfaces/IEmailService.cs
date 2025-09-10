
using Kutiyana_Memon_Hospital_Api.DTOs.Request;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string toEmail, string resetLink);
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
