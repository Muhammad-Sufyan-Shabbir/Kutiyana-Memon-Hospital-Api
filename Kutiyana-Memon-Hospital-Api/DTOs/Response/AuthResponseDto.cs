namespace Kutiyana_Memon_Hospital_Api.DTOs.Response
{
    public class AuthResponseDto
    {
        public string? Token { get; set; }          
        public string? Email { get; set; }      
        public string? UserName { get; set; }         
        public string? FullName { get; set; }       
        public int RoleId { get; set; }               
        public string? RoleName { get; set; }     
        public int CompanyId { get; set; }           
        public string? CompanyName { get; set; }     
        public List<string>? Permissions { get; set; }  
    }
}
