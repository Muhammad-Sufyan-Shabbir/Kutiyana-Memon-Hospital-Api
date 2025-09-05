using Kutiyana_Memon_Hospital_Api.API.Entities;

namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{
    public class RoleResponse : BaseEntity
    { 
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; } 
    }
     
    public class RoleDetailResponse : RoleResponse
    {
        public List<RoleModuleAccessResponse> ModuleAccesses { get; set; } = new();
    }
     
    public class RoleModuleAccessResponse
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
