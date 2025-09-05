namespace Kutiyana_Memon_Hospital_Api.DTOs.Request
{
    public class CompanyRequest
    {
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ContactInfo { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
