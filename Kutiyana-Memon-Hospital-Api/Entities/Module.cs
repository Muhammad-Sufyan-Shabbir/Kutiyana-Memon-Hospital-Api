namespace Kutiyana_Memon_Hospital_Api.API.Entities
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        //Navigation ke liye Parent-Child Menu
        public int? ParentId { get; set; } 
        public Module? Parent { get; set; }
        public ICollection<Module>? Children { get; set; }

        //Frontend/Backend route
        public string? Url { get; set; }

        //UI Icon (optional)
        public string? Icon { get; set; }

        //Order in Menu
        public int OrderNo { get; set; } = 0;

        //Relationship with RoleModuleAccess
        public ICollection<RoleModuleAccess>? RoleModuleAccesses { get; set; }
    }
}