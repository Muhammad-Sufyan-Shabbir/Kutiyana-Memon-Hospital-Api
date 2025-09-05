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
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService service)
        {
            _companyService = service;
        }

        [HttpPost("CreateCompany")]
        public async Task<ActionResult<ResponseModel<CompanyResponse>>> SaveCompanyAsync([FromBody] CompanyRequest request)
        {
            var response = await _companyService.SaveCompanyAsync(request);
            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetCompanyById")]
        public async Task<ActionResult<ResponseModel<CompanyResponse>>> GetCompanyById(int id)
        {
            var response = await _companyService.GetCompanyByIdAsync(id);

            if (response.HttpStatusCode == 200) return Ok(response);
            if (response.HttpStatusCode == 400) return BadRequest(response);

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpDelete("DeleteCompany")]
        public async Task<ActionResult<ResponseModel<bool>>> DeleteCompany(int id)
        {
            var response = await _companyService.DeleteCompanyAsync(id);

            if (response.HttpStatusCode == 200) return Ok(response);
            if (response.HttpStatusCode == 404) return NotFound(response);

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpGet("GetAllCompanies")]
        public async Task<ActionResult<ResponseModel<IEnumerable<CompanyResponse>>>> GetAllCompanies()
        {
            var response = await _companyService.GetAllCompaniesAsync();

            if (response.HttpStatusCode == 200) return Ok(response);

            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
