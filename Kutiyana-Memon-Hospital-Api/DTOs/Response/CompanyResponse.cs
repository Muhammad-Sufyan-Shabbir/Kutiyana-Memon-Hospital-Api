using Kutiyana_Memon_Hospital_Api.API.Entities;

namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{ 
    public class CompanyResponse : BaseEntity
    {            
        public string? Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ContactInfo { get; set; }
    }
}
