
using AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Data;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Repositories.Implementation;
using Kutiyana_Memon_Hospital_Api.API.Repositories.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;

namespace Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IGenericRepository<User> _users;
        public IGenericRepository<User> Users =>
            _users ??= new GenericRepository<User>(_context);

        private IGenericRepository<Role> _roles;
        public IGenericRepository<Role> Roles =>
            _roles ??= new GenericRepository<Role>(_context);

        private ICompanyRepository _companies;
        public ICompanyRepository companyRepository =>
            _companies ??= new CompanyRepository(_context, _mapper);


        private IRoleRepository _roleRepository;
        public IRoleRepository roleRepository =>
            _roleRepository ??= new RoleRepository(_context, _mapper);

        private IGenericRepository<RoleModuleAccess> _roleModuleAccess;
        public IGenericRepository<RoleModuleAccess> roleModuleAccessRepository =>
            _roleModuleAccess ??= new GenericRepository<RoleModuleAccess>(_context);

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}
