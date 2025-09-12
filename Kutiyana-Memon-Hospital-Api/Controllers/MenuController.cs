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
    [Route("api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService service)
        {
            _menuService = service;
        }

        [HttpPost("CreateMenu")]
        public async Task<ActionResult<ResponseModel<MenuResponse>>> SaveMenu([FromBody] MenuRequest request)
        {
            var response = await _menuService.SaveMenuAsync(request); 
            return Ok(response); 
        }

        [HttpGet("GetMenuById")]
        public async Task<ActionResult<ResponseModel<MenuResponse>>> GetMenuById(int id)
        {
            var response = await _menuService.GetMenuByIdAsync(id);
            if (response.HttpStatusCode == 404)
                return NotFound(response);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMenus()
        {
            var response = await _menuService.GetAllMenusAsync();
            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpDelete("DeleteMenuById")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var response = await _menuService.DeleteMenuAsync(id);
            return StatusCode(response.HttpStatusCode, response);
        }


    }
}
