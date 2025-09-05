using System.Security.Claims;

namespace Kutiyana_Memon_Hospital_Api.API.Services.GetCurrentUser
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    Console.WriteLine("HttpContext is null in UserContextService.");
                    return null;
                }

                var userIdClaim = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
            }
        }


    }
}
