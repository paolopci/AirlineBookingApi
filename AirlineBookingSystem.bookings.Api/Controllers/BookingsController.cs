using AirlineBookingSystem.Bookings.Application.Commands;
using AirlineBookingSystem.Bookings.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.bookings.Api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingCommand command)
        {
            var id=await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookingById), new { id }, command);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var query = new GetBookingQuery(id); // Ensure the type is defined in the correct namespace
            var booking = await _mediator.Send(query);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }
    }
}
