using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using AirlineBookingSystem.Payments.Application.Commands;
using MassTransit;
using MassTransit.Mediator;


namespace AirlineBookingSystem.Payments.Application.Consumers
{
    public class FlightBookedConsumer:IConsumer<FlightBookedEvent>
    {
        private readonly IMediator _mediator;

        public FlightBookedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async  Task Consume(ConsumeContext<FlightBookedEvent> context)
        {
            var flightBookedEvent = context.Message;
            var command = new ProcessPaymentCommand(flightBookedEvent.BookingId, 200.00m);
           
            await _mediator.Send(command);
            await Task.CompletedTask; // Simulate async operation, if needed
        }
    }
}
