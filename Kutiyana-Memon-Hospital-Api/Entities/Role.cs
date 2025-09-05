namespace Kutiyana_Memon_Hospital_Api.API.Entities
{
    public class Role : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        // Navigation
        public ICollection<User>? Users { get; set; }   // 👈 Add this
        public ICollection<RoleModuleAccess>? ModuleAccesses { get; set; }
    }
}