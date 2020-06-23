using Ascetic.Microservices.Application.Managers;

namespace Ascetic.Microservices.Application.Extensions
{
    public static class UserContextExtensions
    {
        public static string GetCurrentUserEmail(this IUserContextManager userContextManager)
        {
            var currentUser = userContextManager.GetCurrentUser();
            return currentUser.FindFirst("preferred_username").Value;
        }
    }
}
