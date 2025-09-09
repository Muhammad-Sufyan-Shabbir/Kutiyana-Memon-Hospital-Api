using AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Data;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Repositories.Interfaces;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace Kutiyana_Memon_Hospital_Api.API.Repositories.Implementation
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        private readonly IMapper _mapper;

        public AuthRepository(ApplicationDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public IQueryable<User> GetQueryable()
        {
            return _context.Set<User>().AsQueryable();
        }
    }
}
