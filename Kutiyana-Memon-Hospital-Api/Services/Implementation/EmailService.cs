using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public async Task SendPasswordResetEmail(string toEmail, string resetLink)
        //{
        //    if (string.IsNullOrWhiteSpace(toEmail))
        //        throw new ArgumentException("Recipient email is null or empty.", nameof(toEmail));

        //    if (string.IsNullOrWhiteSpace(resetLink))
        //        throw new ArgumentException("Reset link is null or empty.", nameof(resetLink));

        //    var smtpSettings = _configuration.GetSection("SmtpSettings");
        //    if (smtpSettings == null)
        //        throw new Exception("SMTP configuration section missing in appsettings.json");

        //    string fromEmail = smtpSettings["FromEmail"];
        //    string host = smtpSettings["Host"];
        //    string username = smtpSettings["Username"];
        //    string password = smtpSettings["Password"];
        //    string portStr = smtpSettings["Port"];

        //    if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(host) ||
        //        string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
        //        string.IsNullOrWhiteSpace(portStr))
        //        throw new Exception("SMTP settings are not properly configured.");

        //    if (!int.TryParse(portStr, out int port))
        //        throw new Exception("SMTP Port is not valid.");

        //    using var smtp = new SmtpClient(host, port)
        //    {
        //        EnableSsl = true,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(username, password)
        //    };

        //    using var message = new MailMessage(new MailAddress(fromEmail, "Kutiyana Memon Hospital"), new MailAddress(toEmail))
        //    {
        //        Subject = "Password Reset Request",
        //        Body = $"Click the link below to reset your password:\n\n{resetLink}\n\nThis link will expire in 15 minutes.",
        //        IsBodyHtml = false
        //    };

        //    try
        //    {
        //        await smtp.SendMailAsync(message);
        //        Console.WriteLine($"✅ Password reset email sent to: {toEmail}");
        //    }
        //    catch (SmtpException smtpEx)
        //    {
        //        throw new Exception($"SMTP Error ({smtpEx.StatusCode}): {smtpEx.Message}", smtpEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Email send failed: {ex.Message}", ex);
        //    }
        //}


        public async Task SendResetPasswordEmailAsync(string toEmail, string resetLink)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtpSettings["FromName"], smtpSettings["FromEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Password Reset Request";
            email.Body = new TextPart("html")
            {
                Text = $@"
                <p>Hello,</p>
                <p>Click the button below to reset your password:</p>
                <a href='{resetLink}' style='padding:10px 20px;background-color:blue;color:white;text-decoration:none;border-radius:5px;'>Reset Password</a>
                <p>If you didn't request a password reset, ignore this email.</p>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpSettings["Host"], int.Parse(smtpSettings["Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
