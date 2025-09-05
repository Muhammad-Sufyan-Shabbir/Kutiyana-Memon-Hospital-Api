namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public string Label { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
