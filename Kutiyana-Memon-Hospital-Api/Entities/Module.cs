namespace Kutiyana_Memon_Hospital_Api.API.Entities
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<RoleModuleAccess>? RoleModuleAccesses { get; set; }
    }
}