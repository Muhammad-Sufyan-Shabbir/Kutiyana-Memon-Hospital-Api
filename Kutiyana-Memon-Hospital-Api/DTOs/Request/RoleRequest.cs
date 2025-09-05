namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{ 
    public class RoleRequest
    {
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CompanyId { get; set; }

        public List<RoleModuleAccessRequest> ModuleAccesses { get; set; } = new();
    }
}
