
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<ResponseModel<CompanyResponse>> SaveCompanyAsync(CompanyRequest request);
        Task<ResponseModel<CompanyResponse>> GetCompanyByIdAsync(int id);
        Task<ResponseModel<bool>> DeleteCompanyAsync(int id);
        Task<ResponseModel<IEnumerable<CompanyResponse>>> GetAllCompaniesAsync();
    }
}
