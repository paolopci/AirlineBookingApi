using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBookingSystem.Flights.Core.Entities;


namespace AirlineBookingSystem.Flights.Core.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetFlightsAsync();
        Task AddFlightAsync(Flight flight);
        Task DeleteFlightAsync(Guid id);
    }
}
