using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Repositories.Interfaces;

namespace Kutiyana_Memon_Hospital_Api.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IQueryable<User> GetQueryable();
    }
}
