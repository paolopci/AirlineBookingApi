using AirlineBookingSystem.Flights.Application.Commands;
using AirlineBookingSystem.Flights.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Flights.Api.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights()
        {
            var query = new GetAllFlightsQuery();
            var flights = await _mediator.Send(query);
            return Ok(flights);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody] CreateFlightCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid flight data.");
            }
            var flightId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetFlights), new { id = flightId }, command);
        }

        [HttpDelete]
        [Route("{id:guid}")] 
        public async Task<IActionResult> DeleteFlight(Guid id) 
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid flight ID.");
            }
            await _mediator.Send(new DeleteFlightCommand(id));
            return NoContent();
        }
    }
}
