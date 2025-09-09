using Kutiyana_Memon_Hospital_Api.API.Services.Implementation;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 

namespace RestaurantPOS.API.Controllers
{
    //[Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> SaveUserAsync([FromBody] ApplicationUserRequest request)
        {
            if (request == null)
                return BadRequest(new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = "Invalid request.",
                    HttpStatusCode = 400,
                    Errors = new List<string> { "Request body cannot be null." }
                });

            var response = await _userService.SaveUserAsync(request);

            // Return BadRequest if service indicates failure
            if (response.HttpStatusCode == 400)
                return BadRequest(response);

            return StatusCode(response.HttpStatusCode, response);
        }


        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseModel<object>>> GetById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            return StatusCode(result.HttpStatusCode, result);
        }

    }
}
