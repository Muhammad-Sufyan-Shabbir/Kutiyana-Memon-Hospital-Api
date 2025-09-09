
using Kutiyana_Memon_Hospital_Api.API.Entities; 

namespace Kutiyana_Memon_Hospital_Api.API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, Role role, List<RoleModuleAccess> moduleAccesses);
    }
}
