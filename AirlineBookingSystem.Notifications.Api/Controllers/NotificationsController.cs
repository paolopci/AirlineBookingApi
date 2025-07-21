using AirlineBookingSystem.Notifications.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Notifications.Api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Notify([FromBody] SendNotificationCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid notification command.");
            }
            await _mediator.Send(command);
            return Ok("Notification sent successfully."); 

        }
    }
}
