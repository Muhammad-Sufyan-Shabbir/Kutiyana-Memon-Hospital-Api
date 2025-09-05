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
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult<ResponseModel<RoleResponse>>> SaveCompanyAsync([FromBody] RoleRequest request)
        {
            var response = await _roleService.SaveRoleAsync(request);
            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseModel<object>>> GetById(int id)
        {
            var response = await _roleService.GetRoleByIdAsync(id);

            return StatusCode(response.HttpStatusCode, response);
        }


        [HttpDelete("DeleteRoleById")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            if (response.HttpStatusCode == 404)
                return NotFound(response);
            if (response.HttpStatusCode == 500)
                return StatusCode(500, response);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();
            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
