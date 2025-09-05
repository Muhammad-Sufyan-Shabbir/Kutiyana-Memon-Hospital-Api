
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseModel<RoleDetailResponse>> SaveRoleAsync(RoleRequest request);
        Task<ResponseModel<object>> GetRoleByIdAsync(int roleId);
        Task<ResponseModel<bool>> DeleteRoleAsync(int roleId);
        Task<ResponseModel<IEnumerable<Role>>> GetAllRolesAsync();
    }
}
