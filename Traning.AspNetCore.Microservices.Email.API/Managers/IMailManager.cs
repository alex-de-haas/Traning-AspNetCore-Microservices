using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Email.API.Managers
{
    public interface IMailManager
    {
        Task Send(string email, string subject, string body);
    }
}
