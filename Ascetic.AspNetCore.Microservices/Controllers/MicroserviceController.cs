using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace Ascetic.AspNetCore.Microservices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MicroserviceController : ControllerBase
    {
        /// <summary>
        /// Get microservice version.
        /// </summary>
        /// <response code="200">Returns microservice version.</response>
        [HttpGet("version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyVersion = assembly?.GetName().Version.ToString();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            return Ok(new
            {
                ProductVersion = fileVersionInfo.ProductVersion,
                FileVersion = fileVersionInfo.FileVersion,
                AssemblyVersion = assemblyVersion
            });
        }
    }
}
