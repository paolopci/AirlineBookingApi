using AirlineBookingSystem.Bookings.Application.Commands;
using AirlineBookingSystem.Bookings.Core.Entities;
using AirlineBookingSystem.Bookings.Core.Repositories;
using MassTransit;
using MediatR;


namespace AirlineBookingSystem.Bookings.Application.Handles
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IBookingRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateBookingHandler(IBookingRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                FlightId = request.FlightId,
                PassengerName = request.PassengerName,
                SeatNumber = request.SeatNumber,
                BookingDate = DateTime.UtcNow
            };

            await _repository.AddBookingAsync(booking);
            return booking.Id;
        }
    }
}
