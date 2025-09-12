
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IMenuService
    {
        Task<ResponseModel<MenuResponse>> SaveMenuAsync(MenuRequest request);
        Task<ResponseModel<MenuResponse>> GetMenuByIdAsync(int id);
        Task<ResponseModel<List<MenuResponse>>> GetAllMenusAsync();
        Task<ResponseModel<bool>> DeleteMenuAsync(int id);
    }
}
