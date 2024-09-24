using System;
using System.Threading.Tasks;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]"),]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        
        [HttpPost]
        public async Task<IActionResult> TicketReservation([FromBody] TicketReservationRequestDto request)
        {
            try
            {
                var ticketReservation = await _ticketService.TicketReservation(request);

                return Ok(ticketReservation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut, Route("confirmation/{id}")]
        public async Task<IActionResult> TicketConfirmation(Guid id)
        {
            try
            {
                var ticketConfirmation = await _ticketService.TicketConfirmation(id);

                return Ok(ticketConfirmation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}