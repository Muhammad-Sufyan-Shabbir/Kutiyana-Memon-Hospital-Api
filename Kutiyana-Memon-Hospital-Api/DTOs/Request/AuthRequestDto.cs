namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{
    public class AuthRequestDto
    {
        public string? EmailOrUsername { get; set; }
        public string? Password { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string? Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
