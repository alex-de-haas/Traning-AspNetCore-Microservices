using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Email.API.Managers
{
    public class MailManager : IMailManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailManager> _logger;

        public MailManager(IConfiguration configuration, ILogger<MailManager> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task Send(string email, string subject, string body)
        {
            _logger.LogInformation($"Email:{Environment.NewLine}{email}{Environment.NewLine}{subject}{Environment.NewLine}{body}");
            return Task.CompletedTask;
        }
    }
}
