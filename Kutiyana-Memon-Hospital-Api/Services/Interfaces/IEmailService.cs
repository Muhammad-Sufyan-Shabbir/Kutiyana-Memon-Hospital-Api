 
namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string toEmail, string resetLink);
    }
}
