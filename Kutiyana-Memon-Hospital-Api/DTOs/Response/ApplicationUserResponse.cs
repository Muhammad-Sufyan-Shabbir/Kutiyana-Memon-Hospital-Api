using Kutiyana_Memon_Hospital_Api.API.Entities;

namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{
    public class ApplicationUserResponse : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public int CompanyId { get; set; }
        public Company? company { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public DateTime? JoiningDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
