namespace Kutiyana_Memon_Hospital_Api.API.Entities
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }  
        public string? Email { get; set; }      
        public string? Phone { get; set; }       
        public string? PasswordHash { get; set; }
        public string? ImageUrl { get; set; }
        public int CompanyId { get; set; }
        public Company? company { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public DateTime? JoiningDate { get; set; }
        public bool IsActive { get; set; } = true; 
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}".Trim();
            }
        }
        public string Label
        {
            get
            {
                var firstInitial = !string.IsNullOrWhiteSpace(FirstName) ? FirstName[0].ToString().ToUpper() : "";
                var lastInitial = !string.IsNullOrWhiteSpace(LastName) ? LastName[0].ToString().ToUpper() : "";
                return $"{firstInitial}{lastInitial}";
            }
        }
    }
}
