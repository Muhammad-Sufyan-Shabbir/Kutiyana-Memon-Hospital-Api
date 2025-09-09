using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.Repositories.Interfaces
{
    public interface IFunctionRepository
    {
        Task<object> GetRoleByIdAsync(int roleId);
        Task<object> GetUserByIdAsync(int userId);
        Task<PageResponse<object>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
    }
}
