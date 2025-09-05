namespace Kutiyana_Memon_Hospital_Api.Repositories.Interfaces
{
    public interface IFunctionRepository
    {
        Task<object> GetRoleByIdAsync(int roleId);
    }
}
