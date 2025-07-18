using System.Data;
using AirlineBookingSystem.Notifications.Core.Entities;
using AirlineBookingSystem.Notifications.Core.Repositories;
using Dapper;


namespace AirlineBookingSystem.Notifications.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnection _dbConnection;

        public NotificationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task LogNotificationAsync(Notification notification)
        {
            const string sql = @"
                insert into Notifications (Id, Recipient, Message, Type, SentAt)  
                VALUES (@Id, @Recipient, @Message, @Type, @SentAt )
               ";

            await _dbConnection.ExecuteAsync(sql, notification);
        }
    }
}
