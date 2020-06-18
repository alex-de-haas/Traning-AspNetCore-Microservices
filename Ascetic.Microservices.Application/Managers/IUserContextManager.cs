using System.Security.Claims;

namespace Ascetic.Microservices.Application.Managers
{
    public interface IUserContextManager
    {
        ClaimsPrincipal GetCurrentUser();
    }
}
