

using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Repositories.Interfaces;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;

namespace Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        ICompanyRepository companyRepository { get; }
        IRoleRepository roleRepository { get; }
        IGenericRepository<RoleModuleAccess> roleModuleAccessRepository { get; }
        IUserRepository userRepository { get; }
        IAuthRepository authRepository { get; }
        IMenuRepository menuRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
