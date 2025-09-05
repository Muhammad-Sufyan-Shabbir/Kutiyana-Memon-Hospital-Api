using Kutiyana_Memon_Hospital_Api.API.Modals;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response; 
using Microsoft.AspNetCore.Mvc;

namespace Kutiyana_Memon_Hospital_Api.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>(result));
        }
    }
}
