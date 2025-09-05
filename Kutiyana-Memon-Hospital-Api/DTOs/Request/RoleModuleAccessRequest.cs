namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{
    public class RoleModuleAccessRequest
    {
        public int ModuleId { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
