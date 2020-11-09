using Common.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Service.Command;
using Persistence.Service.Query;
using System;
using System.Threading.Tasks;

namespace Service.Queue.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoggerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _mediator.Send(new GetLoggerByIdQuery
                {
                    Id = id
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetLoggerByFilterQuery filter)
        {
            try
            {
                return Ok(await _mediator.Send(filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Logger logger)
        {
            try
            {
                return Ok(await _mediator.Send(new CreateLoggerCommand
                {
                    Logger = logger
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
