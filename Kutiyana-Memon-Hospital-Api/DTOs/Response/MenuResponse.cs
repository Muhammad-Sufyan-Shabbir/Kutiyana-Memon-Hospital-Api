using Kutiyana_Memon_Hospital_Api.API.Entities;

namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{ 
    public class MenuResponse : BaseEntity
    { 
        public string Name { get; set; } = string.Empty;

        // Parent Info (agar parent null ho to root module hoga)
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }

        // Child Modules
        public List<MenuResponse>? Children { get; set; }

        // Routes & UI
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int OrderNo { get; set; }

        // Optional: Role-wise access (sirf zarurat ho to)
        public List<string>? Roles { get; set; }
    }
}
