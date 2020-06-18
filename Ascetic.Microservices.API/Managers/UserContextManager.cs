using Ascetic.Microservices.Application.Managers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ascetic.Microservices.API.Managers
{
    public class UserContextManager : IUserContextManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.User;
        }
    }
}
