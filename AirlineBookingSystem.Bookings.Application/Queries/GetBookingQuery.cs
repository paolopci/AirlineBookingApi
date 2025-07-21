using AirlineBookingSystem.Bookings.Core.Entities;
using MediatR;


namespace AirlineBookingSystem.Bookings.Application.Queries
{
    public record GetBookingQuery(Guid id):IRequest<Booking>;


}
