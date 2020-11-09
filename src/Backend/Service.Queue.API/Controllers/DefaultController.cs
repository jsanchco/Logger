using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.Queue.API.Controllers
{
    [ApiController]
    [Route("api/v1/default")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Index()
        {
            _logger.LogInformation("In DefaultController [HttpGet]");
            return "Running ..";
        }
    }
}
