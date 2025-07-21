using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBookingSystem.Flights.Core.Entities;
using MediatR;


namespace AirlineBookingSystem.Flights.Application.Queries
{
    public record GetAllFlightsQuery : IRequest<IEnumerable<Flight>>;


}
