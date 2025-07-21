using System.ComponentModel.Design;
using AirlineBookingSystem.Payments.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Payments.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid payment request.");
            }
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(ProcessPayment), new { id = result }, command);
        }

        [HttpPost("refun/{id}")]
        public async Task<IActionResult> RefundPayment(Guid id)
        {
            var command = new RefundPaymentCommand(id);
            await _mediator.Send(command);
            return NoContent(); // 204 No Content
        }
    }
}
