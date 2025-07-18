using  AirlineBookingSystem.Flights.Core.Entities;
using AirlineBookingSystem.Flights.Core.Repositories;
using System.Data;
using Dapper;


namespace AirlineBookingSystem.Flights.Infrastructure.Repositories
{
    public  class FlightRepository: IFlightRepository
    {
        private readonly IDbConnection _dbConnection;

        public FlightRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            const string sql= "SELECT * FROM Flights";

            return _dbConnection.QueryAsync<Flight>(sql);
        }

        public async Task AddFlightAsync(Flight flight)
        {
            const string sql = @"
                insert into Flights (Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime)
                values (@Id, @FlightNumber, @Origin, @Destination, @DepartureTime, @ArrivalTime)
                ";
            await _dbConnection.ExecuteAsync(sql, new
            {
                flight.Id,
                flight.FlightNumber,
                flight.Origin,
                flight.Destination,
                flight.DepartureTime,
                flight.ArrivalTime
            });
        }

        public Task DeleteFlightAsync(Guid id)
        {
            const string sql =  "DELETE FROM Flights WHERE Id = @Id";

            return _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
