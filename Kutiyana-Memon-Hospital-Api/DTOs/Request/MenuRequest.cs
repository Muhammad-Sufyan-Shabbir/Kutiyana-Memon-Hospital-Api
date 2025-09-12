namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{
    public class MenuRequest
    {
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int? ParentId { get; set; }  
        public string? Url { get; set; } 
        public string? Icon { get; set; } 
        public int OrderNo { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
