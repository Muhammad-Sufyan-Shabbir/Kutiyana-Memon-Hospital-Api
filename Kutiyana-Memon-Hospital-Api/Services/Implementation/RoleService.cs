using AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;
using Kutiyana_Memon_Hospital_Api.Repositories.Implementation;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFunctionRepository _functionRepository;


        public RoleService(IUnitOfWork uow, IMapper mapper, IFunctionRepository functionRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _functionRepository = functionRepository;
        }

        public async Task<ResponseModel<RoleDetailResponse>> SaveRoleAsync(RoleRequest request)
        {
            try
            {
                Role role;
                 
                if (request.GlobalId == Guid.Empty)
                {
                    role = _mapper.Map<Role>(request);
                    role.GlobalId = Guid.NewGuid();

                    await _uow.roleRepository.AddAsync(role);
                    await _uow.SaveChangesAsync();

                    return new ResponseModel<RoleDetailResponse>
                    {
                        Result = _mapper.Map<RoleDetailResponse>(role),
                        Message = "Role created successfully.",
                        HttpStatusCode = 201
                    };
                }
                 
                role = await _uow.roleRepository
                 .GetQueryable()
                 .Include(r => r.ModuleAccesses)
                     .ThenInclude(ma => ma.Module)
                 .FirstOrDefaultAsync(r => r.GlobalId == request.GlobalId);

                if (role == null)
                    return new ResponseModel<RoleDetailResponse>
                    {
                        Result = null,
                        Message = "Role not found with the given GlobalId.",
                        HttpStatusCode = 404
                    };

                role.Name = request.Name ?? role.Name;
                role.Description = request.Description ?? role.Description;
                role.CompanyId = request.CompanyId;

                if (request.ModuleAccesses?.Any() == true)
                {
                    role.ModuleAccesses.Clear();

                    foreach (var moduleAccess in request.ModuleAccesses)
                    {
                        role.ModuleAccesses.Add(new RoleModuleAccess
                        {
                            ModuleId = moduleAccess.ModuleId,
                            CanRead = moduleAccess.CanRead,   
                            CanCreate = moduleAccess.CanCreate,
                            CanUpdate = moduleAccess.CanUpdate,
                            CanDelete = moduleAccess.CanDelete
                        });
                    }
                }

                _uow.roleRepository.Update(role);
                await _uow.SaveChangesAsync();

                return new ResponseModel<RoleDetailResponse>
                {
                    Result = _mapper.Map<RoleDetailResponse>(role),
                    Message = "Role updated successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<RoleDetailResponse>
                {
                    Result = null,
                    Message = $"Error saving role: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }


        public async Task<ResponseModel<object>> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var roleDetail = await _functionRepository.GetRoleByIdAsync(roleId);

                return new ResponseModel<object>
                {
                    Result = roleDetail,
                    Message = roleDetail == null ? "Role not found." : "Role retrieved successfully.",
                    HttpStatusCode = roleDetail == null ? 404 : 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Result = null,
                    Message = $"Error fetching role: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteRoleAsync(int roleId)
        {
            try
            {
                var role = await _uow.roleRepository
                    .GetQueryable()
                    .Include(r => r.ModuleAccesses)
                    .FirstOrDefaultAsync(r => r.Id == roleId);

                if (role == null)
                {
                    return new ResponseModel<bool>
                    {
                        Result = false,
                        Message = "Role not found.",
                        HttpStatusCode = 404
                    };
                }

                // Pehle role ke parameters (roleModuleAccess) delete karo
                if (role.ModuleAccesses != null && role.ModuleAccesses.Any())
                {
                    foreach (var access in role.ModuleAccesses.ToList())
                    {
                        _uow.roleModuleAccessRepository.Delete(access);
                    }
                }

                // Ab role delete karo
                _uow.roleRepository.Delete(role);

                await _uow.SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    Result = true,
                    Message = "Role and its related accesses deleted successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Result = false,
                    Message = $"Error deleting role: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }


        public async Task<ResponseModel<IEnumerable<Role>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _uow.roleRepository.GetAllAsync();

                return new ResponseModel<IEnumerable<Role>>
                {
                    Result = roles,
                    Message = "Roles retrieved successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<Role>>
                {
                    Result = null,
                    Message = $"Error fetching roles: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }



    }
}
