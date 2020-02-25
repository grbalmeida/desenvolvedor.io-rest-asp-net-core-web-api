using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : MainController
    {
        private readonly ILogger _logger;

        public TestController(INotifier notifier, IUser user, ILogger<TestController> logger) : base(notifier, user)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Value()
        {
            _logger.LogTrace("Log Trace"); // development
            _logger.LogDebug("Log Debug");
            _logger.LogInformation("Log Information");
            _logger.LogWarning("Log Warning"); // Ex: error 404
            _logger.LogError("Log Error");
            _logger.LogCritical("Log Critical"); // one level above the error

            return "I'm V2";
        }
    }
}