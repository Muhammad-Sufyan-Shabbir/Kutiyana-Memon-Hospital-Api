using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;
using System.Net.Mail; 

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmail(string toEmail, string resetLink)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Recipient email is null or empty.", nameof(toEmail));

            if (string.IsNullOrWhiteSpace(resetLink))
                throw new ArgumentException("Reset link is null or empty.", nameof(resetLink));

            var smtpSettings = _configuration.GetSection("SmtpSettings");
            if (smtpSettings == null)
                throw new Exception("SMTP configuration section missing in appsettings.json");

            string fromEmail = smtpSettings["FromEmail"];
            string username = smtpSettings["UserName"];
            string password = smtpSettings["Password"];
            string host = smtpSettings["Host"];
            string portStr = smtpSettings["Port"];

            if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(portStr))
                throw new Exception("SMTP settings are not properly configured.");

            if (!int.TryParse(portStr, out int port))
                throw new Exception("SMTP Port is not valid.");

            string htmlBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .card {{
                            max-width: 500px;
                            margin: 40px auto;
                            background: #fff;
                            border-radius: 12px;
                            border: 1px solid #e0e0e0;
                            box-shadow: 0 6px 18px rgba(0,0,0,0.1);
                            padding: 30px;
                            text-align: center;
                        }}
                        .logo {{
                            margin-bottom: 20px;
                        }}
                        .logo img {{
                            max-width: 120px;
                        }}
                        .card h2 {{
                            color: #2c3e50;
                            margin: 0 0 20px 0;
                        }}
                        .card p {{
                            color: #555;
                            font-size: 15px;
                            line-height: 1.6;
                            margin: 10px 0;
                        }}
                        .btn {{
                            display: inline-block;
                            margin-top: 20px;
                            margin-bottom: 15px;
                            padding: 7px 20px;
                            background-color: #1d37ab;
                            color: #fff !important;
                            text-decoration: none;
                            font-size: 15px;
                            font-weight: bold;
                            border-radius: 6px;
                        }}
                        .btn:hover {{
                            background-color: #162b85;
                        }}
                        .footer {{
                            margin-top: 25px;
                            font-size: 12px;
                            color: #999;
                        }}
                    </style>
                </head>
                <body>
                    <div class='card'>
                        <div class='logo'>
                            <div style='text-align:center;'>
                                <img src='' alt='KMH Logo' style='max-width:100px;' />
                            </div>
                        </div>
                        <h2>Password Reset Request</h2>
                        <p>We received a request to reset your password.<br/>
                        Click the button below to reset it.</p>
                        <a href='{resetLink}' class='btn'>Reset Password</a>
                        <p class='footer'>
                            This link will expire in 15 minutes.<br/>
                            If you didn’t request this, you can safely ignore this email.
                        </p>
                    </div>
                </body>
                </html>";
            using var message = new MailMessage(
                new MailAddress(fromEmail, "Kutiyana Memon Hospital"),
                new MailAddress(toEmail)
            )
            {
                Subject = "Password Reset Request",
                Body = htmlBody,
                IsBodyHtml = true  
            };

            using var smtp = new System.Net.Mail.SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            try
            {
                await smtp.SendMailAsync(message);
                Console.WriteLine($"✅ Password reset email sent to: {toEmail}");
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception($"SMTP Error ({smtpEx.StatusCode}): {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Email send failed: {ex.Message}", ex);
            }
        }
         
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var fromAddress = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]);
            var toAddress = new MailAddress(emailRequest.ToEmail);

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailRequest.Subject,
                Body = emailRequest.Body,
                IsBodyHtml = true
            };

            using var smtp = new System.Net.Mail.SmtpClient
            {
                Host = smtpSettings["Host"],
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    smtpSettings["UserName"],
                    smtpSettings["Password"]
                )
            };
            await smtp.SendMailAsync(message);
        }


    }
}
