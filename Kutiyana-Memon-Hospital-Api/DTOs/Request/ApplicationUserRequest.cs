namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{ 
    public class ApplicationUserRequest
    {
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public DateTime? JoiningDate { get; set; }
        public bool? IsActive { get; set; }
    }

}
