using Common.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceList;

namespace Service.Queue.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IServiceList<Logger> _serviceList;

        public QueueController(IServiceList<Logger> serviceList)
        {
            _serviceList = serviceList;
        }

        [HttpPost]
        public IActionResult Add([FromBody] Logger logger)
        {
            _serviceList.Add(logger);

            return Ok($"{_serviceList.Count()} items in List");
        }
    }
}
