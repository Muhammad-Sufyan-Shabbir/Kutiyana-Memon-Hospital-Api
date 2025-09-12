using AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;

namespace Kutiyana_Memon_Hospital_Api.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<ApplicationUserRequest, User>();
            CreateMap<User, ApplicationUserResponse>();

            // Role → Request/Response
            CreateMap<RoleRequest, Role>()
                .ForMember(dest => dest.ModuleAccesses, opt => opt.MapFrom(src => src.ModuleAccesses));

            CreateMap<Role, RoleResponse>();

            CreateMap<Role, RoleDetailResponse>()
                .ForMember(dest => dest.ModuleAccesses, opt => opt.MapFrom(src => src.ModuleAccesses));

            // Module Access
            CreateMap<RoleModuleAccessRequest, RoleModuleAccess>();

            CreateMap<RoleModuleAccess, RoleModuleAccessResponse>()
                .ForMember(dest => dest.ModuleName,
                           opt => opt.MapFrom(src => src.Module != null ? src.Module.Name : string.Empty));

            // Company
            CreateMap<CompanyRequest, Company>();
            CreateMap<Company, CompanyResponse>();

            // ✅ Module Mapping
            CreateMap<MenuRequest, Module>();
            CreateMap<Module, MenuResponse>()
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.RoleModuleAccesses != null
                        ? src.RoleModuleAccesses.Select(r => r.Role.Name).ToList()
                        : new List<string>()
                ));
        }
    }
}
