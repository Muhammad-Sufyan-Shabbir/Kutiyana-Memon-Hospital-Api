
using Kutiyana_Memon_Hospital_Api.API.Entities;
using System.Linq.Expressions;

namespace Kutiyana_Memon_Hospital_Api.API.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByGlobalIdAsync(Guid globalId);
        Task<User?> GetByEmailOrUserNameAsync(string identifier);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
