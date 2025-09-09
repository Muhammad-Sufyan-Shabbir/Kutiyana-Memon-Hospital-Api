
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<ApplicationUserResponse>> SaveUserAsync(ApplicationUserRequest request);
        Task<ResponseModel<object>> GetUserByIdAsync(int userId);
        Task<ResponseModel<object>> GetAllUsersAsync();
        Task<ResponseModel<bool>> DeleteUserAsync(int userId);
    }
}
