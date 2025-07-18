using System.Data;
using AirlineBookingSystem.Bookings.Core.Entities;
using AirlineBookingSystem.Bookings.Core.Repositories;
using Dapper;


namespace AirlineBookingSystem.Bookings.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnection _dbConnection;

        public BookingRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id)
        {
            const string sql = @"SELECT * FROM Bookings WHERE Id = @Id";

            return await _dbConnection.QuerySingleOrDefaultAsync<Booking>(sql, new {Id=id});
        }

        public async Task AddBookingAsync(Booking booking)
        {
            const string sql = @"
            INSERT INTO Bookings (Id, FlightId, PassengerName, SeatNumber, BookingDate)
            VALUES (@Id, @FlightId, @PassengerName, @SeatNumber, @BookingDate)";

            await _dbConnection.ExecuteAsync(sql, new
            {
                booking.Id,
                booking.FlightId,
                booking.PassengerName,
                booking.SeatNumber,
                booking.BookingDate
            });
        }
    }
}
